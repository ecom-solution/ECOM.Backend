using ECOM.App.Extensions;
using ECOM.App.Implementations.BusinessLogics.Common;
using ECOM.App.Interfaces.BusinessLogics;
using ECOM.Domain.Interfaces.Hashs;
using ECOM.App.Interfaces.Loggings;
using ECOM.Domain.Interfaces.OTPs;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.DataContracts;
using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Enums.Entity;
using ECOM.Shared.Library.Exceptions.HttpStatus;
using ECOM.Shared.Library.Models.Dtos.Common;
using ECOM.Shared.Library.Models.Dtos.Modules.Authentication.Users;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ECOM.App.Interfaces.Users;

namespace ECOM.App.Implementations.BusinessLogics
{
	public class AuthService(
		ILog logger,
		IOptions<AppSettings> appSettings,
		[FromKeyedServices(DatabaseConstants.Main)] IUnitOfWork mainUnitOfWork,
		ICurrentUserAccessor currentUserAccessor,
		IPasswordHasher passwordHasher,
		ITotpService totpService) 
		: BaseService(logger, appSettings, mainUnitOfWork, currentUserAccessor), IAuthService
	{
		private readonly IPasswordHasher _passwordHasher = passwordHasher;
		private readonly ITotpService _totpService = totpService;

		public async Task<BaseResponse<UserSignedIn>> SignUpAsync(BaseRequest<UserSignUp> request)
		{
			using var transaction = await _mainUnitOfWork.BeginTransactionAsync();
			try
			{
				var response = new BaseResponse<UserSignedIn>();

				var userSignUp = request.Model;
				if (string.IsNullOrWhiteSpace(userSignUp.UserName) || string.IsNullOrWhiteSpace(userSignUp.Password))
					throw new BadRequestException($"UserName and Password are required.");

				var existingUser = await _mainUnitOfWork.Repository<ApplicationUser>()
														.FirstOrDefaultAsync(x => x.NormalizedUserName == userSignUp.UserName.ToUpper().Trim());
				if (existingUser != null)
					throw new BadRequestException($"UserName {userSignUp.UserName} is already registered.");

				var user = new ApplicationUser()
				{
					UserName = userSignUp.UserName.Trim().ToLower(),
					NormalizedUserName = userSignUp.UserName.Trim().ToUpper(),
					PasswordHash = _passwordHasher.Hash(userSignUp.Password),
					SecurityStamp = Guid.NewGuid().ToString(),
					ConcurrencyStamp = Guid.NewGuid().ToString(),
					TimeZoneId = string.IsNullOrWhiteSpace(userSignUp.TimeZoneId) ? ApplicationConstants.DefaultTimeZoneId : userSignUp.TimeZoneId,
					Currency = string.IsNullOrWhiteSpace(userSignUp.Currency) ? ApplicationConstants.DefaultCurrency : userSignUp.Currency,
					Language = string.IsNullOrWhiteSpace(userSignUp.Language) ? ApplicationConstants.DefaultLanguage : userSignUp.Language,
				};

				await _mainUnitOfWork.Repository<ApplicationUser>().InsertAsync(user);
				await _mainUnitOfWork.SaveChangesAsync();

				//Add user to Admin role // test
				var defaultRole = await _mainUnitOfWork.Repository<ApplicationRole>().FirstOrDefaultAsync(x => x.Name == AuthenticationConstants.Role.Admin)
					?? throw new NotFoundException($"Default role not found!");

				var userRole = new ApplicationUserRole(user.Id, defaultRole.Id)
				{
					Role = defaultRole
				};
				await _mainUnitOfWork.Repository<ApplicationUserRole>().InsertAsync(userRole);

				await UpdateRefreshToken(user);

				await _mainUnitOfWork.SaveChangesAsync();

				var accessToken = GenerateAccessToken(user, [defaultRole], []);

				response.Successful(new UserSignedIn()
				{
					Id = user.Id,
					UserName = user.UserName,
					TimeZoneId = user.TimeZoneId,
					Currency = user.Currency,
					Language = user.Language,
					AccessToken = accessToken ?? string.Empty
				});

				await transaction.CommitAsync();

				return response;
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public async Task<BaseResponse<UserSignedIn>> SignInAsync(BaseRequest<UserSignIn> request)
		{
			var userSignIn = request.Model;
			var response = new BaseResponse<UserSignedIn>();

			var normalizedUserName = userSignIn.UserName.ToUpperInvariant().Trim();
			var user = await _mainUnitOfWork.Repository<ApplicationUser>()
											.FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUserName)
												?? throw new NotFoundException($"User {userSignIn.UserName} not found!");

			if (!_passwordHasher.Verify(userSignIn.Password, user.PasswordHash))
			{
				await HandleFailedSignInAttemptAsync(user);
			}

			HandleCheckUserStatus(user);

			var isAdmin = await IsAdminUserAsync(user);
			if (isAdmin)
			{
				await HandleAdminSignInAsync(user, response);
			}
			else
			{
				await HandleNormalUserSignInAsync(user, response);
			}

			await _mainUnitOfWork.SaveChangesAsync();

			return response;
		}

		public async Task<BaseResponse<UserSignedIn>> AdminSignInAsync(BaseRequest<UserSignIn> request)
		{
			var userSignIn = request.Model;
			var response = new BaseResponse<UserSignedIn>();

			var normalizedUserName = userSignIn.UserName.ToUpperInvariant().Trim();
			var user = await _mainUnitOfWork.Repository<ApplicationUser>()
											.FirstOrDefaultAsync(x => x.NormalizedUserName == normalizedUserName)
												?? throw new NotFoundException($"User {userSignIn.UserName} not found!");

			if (!_passwordHasher.Verify(userSignIn.Password, user.PasswordHash))
			{
				await HandleFailedSignInAttemptAsync(user);
			}

			var isAdmin = await IsAdminUserAsync(user);
			if (!isAdmin)
				throw new UnauthorizedException($"User {userSignIn.UserName} is not an admin user!");

			HandleCheckUserStatus(user);

			await HandleAdminSignInAsync(user, response);

			await _mainUnitOfWork.SaveChangesAsync();

			return response;
		}

		public async Task<BaseResponse<UserSignedIn>> AdminVerifyOtpAsync(BaseRequest<UserVerifyOtp> request)
		{
			var response = new BaseResponse<UserSignedIn>();

			var userSignIn = request.Model;
			var user = await _mainUnitOfWork.Repository<ApplicationUser>()
											.FirstOrDefaultAsync(x => x.NormalizedUserName == userSignIn.UserName.ToUpper().Trim() && x.SecretKey == userSignIn.SecretKey)
											 ?? throw new NotFoundException($"User {userSignIn.UserName} not found!");

			var isValid = _totpService.VerifyOtp(userSignIn.OtpCode, user.SecretKey);
			if (isValid)
			{
				await UpdateRefreshToken(user);

				var roles = await GetAllRolesByUserId(user.Id);
				var claims = await GetAllClaimsByUserId(user.Id);
				var accessToken = GenerateAccessToken(user, roles, claims);

				response.Successful(new UserSignedIn
				{
					Id = user.Id,
					UserName = user.UserName,
					TimeZoneId = user.TimeZoneId,
					Currency = user.Currency,
					Language = user.Language,
					AccessToken = accessToken
				});
			}
			else
			{
				user.VerifyFailedCount = (user.VerifyFailedCount ?? 0) + 1;

				if (user.VerifyFailedCount >= _appSettings.Authentication.MaxVerifyFailedCount)
				{
					user.Status = (int)UserStatusEnum.Inactive;
					user.LockoutEndDate_Utc = DateTime.UtcNow.AddDays(_appSettings.Authentication.NumberOfDaysLocked);
					user.LockedReason = "Exceeded failed verify otp attempts";

					response.Failure($"OTP {userSignIn.OtpCode} incorrect!", statusCode: StatusCodes.Status423Locked);
				}
				else
				{
					response.Failure($"OTP {userSignIn.OtpCode} incorrect!", statusCode: StatusCodes.Status401Unauthorized);
				}

				_mainUnitOfWork.Repository<ApplicationUser>().Update(user);

			}

			await _mainUnitOfWork.SaveChangesAsync();

			return response;
		}

		public async Task<BaseResponse<UserSignedOut>> SignOutAsync(BaseRequest<UserSignOut> request)
		{
			var response = new BaseResponse<UserSignedOut>();
			var userSignOut = request.Model;
			var token = await _mainUnitOfWork.Repository<ApplicationUserToken>()
											 .FirstOrDefaultAsync(t => t.UserId == userSignOut.Id &&
																	   t.Provider == ApplicationConstants.AppName &&
																	   t.TokenName == ApplicationConstants.RefreshToken);

			if (token != null)
			{
				_mainUnitOfWork.Repository<ApplicationUserToken>().Delete(token);
				await _mainUnitOfWork.SaveChangesAsync();
			}

			response.Successful(new UserSignedOut
			{
				Id = userSignOut.Id
			});

			return response;
		}

		//---------------------------------------------------------------------------------------------------------------------

		private string GenerateAccessToken(ApplicationUser user, List<ApplicationRole> roles, List<ApplicationClaim> claims)
		{
			var jwtSettings = _appSettings.Authentication.Jwt;
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var tokenClaims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			tokenClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

			tokenClaims.AddRange(claims.Select(claim => new Claim(claim.ClaimType, claim.ClaimValue)));

			var token = new JwtSecurityToken(
				issuer: jwtSettings.ValidIssuer,
				audience: jwtSettings.ValidAudience,
				claims: tokenClaims,
				expires: DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenValidityInMinutes),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private async Task UpdateRefreshToken(ApplicationUser user)
		{
			var existToken = await _mainUnitOfWork.Repository<ApplicationUserToken>().FirstOrDefaultAsync(x => x.UserId == user.Id && x.Provider == ApplicationConstants.AppName && x.TokenName == ApplicationConstants.RefreshToken);
			if (existToken == null)
			{
				var refreshToken = CreateNewRefreshToken(user.Id);
				await _mainUnitOfWork.Repository<ApplicationUserToken>().InsertAsync(refreshToken);
			}
			else
			{
				existToken.TokenValue = GenerateNewRefreshToken();
				existToken.TokenExpiredAt_Utc = DateTime.UtcNow.AddDays(_appSettings.Authentication.Jwt.RefreshTokenValidityInDays);

				_mainUnitOfWork.Repository<ApplicationUserToken>().Update(existToken);
			}
		}

		private ApplicationUserToken CreateNewRefreshToken(Guid userId)
		{
			var token = GenerateNewRefreshToken();

			return new ApplicationUserToken()
			{
				UserId = userId,
				Provider = ApplicationConstants.AppName,
				TokenName = ApplicationConstants.RefreshToken,
				TokenValue = token,
				TokenExpiredAt_Utc = DateTime.UtcNow.AddDays(_appSettings.Authentication.Jwt.RefreshTokenValidityInDays)
			};
		}

		private static string GenerateNewRefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		private async Task<List<ApplicationRole>> GetAllRolesByUserId(Guid userId)
		{
			var roleRepo = _mainUnitOfWork.Repository<ApplicationRole>();
			var userRoleRepo = _mainUnitOfWork.Repository<ApplicationUserRole>();

			var query = userRoleRepo.Where(ur => ur.UserId == userId)
									.Join(roleRepo.Query(), ur => ur.RoleId, r => r.Id, (ur, r) => r);

			return await roleRepo.ToListAsync(query);
		}

		private async Task<List<ApplicationClaim>> GetAllClaimsByUserId(Guid userId)
		{
			var userClaimRepo = _mainUnitOfWork.Repository<ApplicationUserClaim>();
			var userRoleRepo = _mainUnitOfWork.Repository<ApplicationUserRole>();
			var roleClaimRepo = _mainUnitOfWork.Repository<ApplicationRoleClaim>();
			var claimRepo = _mainUnitOfWork.Repository<ApplicationClaim>();

			// Direct user claims
			var directClaims = userClaimRepo.Where(uc => uc.UserId == userId)
											.Join(claimRepo.Query(), uc => uc.ClaimId, c => c.Id, (uc, c) => c);

			// Role-based claims
			var roleClaims = userRoleRepo.Where(ur => ur.UserId == userId)
										 .Join(roleClaimRepo.Query(), ur => ur.RoleId, rc => rc.RoleId, (ur, rc) => rc)
										 .Join(claimRepo.Query(), rc => rc.ClaimId, c => c.Id, (rc, c) => c);

			// Union & distinct
			var unionQuery = directClaims.Union(roleClaims).Distinct();

			return await claimRepo.ToListAsync(unionQuery);
		}

		private string GenerateQRCodeUri(ApplicationUser user)
		{
			user.SecretKey = _totpService.GenerateSecretKey();
			_mainUnitOfWork.Repository<ApplicationUser>().Update(user);

			return _totpService.GenerateQrCodeUri(user.UserName, user.SecretKey);
		}

		private async Task<bool> IsAdminUserAsync(ApplicationUser user)
		{
			return await _mainUnitOfWork.Repository<ApplicationUserRole>().HasRoleAsync(user.Id, AuthenticationConstants.Role.Admin);
		}

		private void HandleCheckUserStatus(ApplicationUser user)
		{
			if (user.Status == (int)UserStatusEnum.Inactive)
			{
				if (user.LockoutEndDate_Utc.HasValue && user.LockoutEndDate_Utc.Value > DateTime.UtcNow)
				{
					throw new UnauthorizedException($"User is currently locked: {user.LockedReason}");
				}

				// Unlock user if lock expired
				user.Status = (int)UserStatusEnum.Active;
				user.AccessFailedCount = 0;
				user.VerifyFailedCount = 0;
				user.LockoutEndDate_Utc = null;

				_mainUnitOfWork.Repository<ApplicationUser>().Update(user);
			}
			else if (user.Status is not ((int)UserStatusEnum.New or (int)UserStatusEnum.Active))
			{
				throw new UnauthorizedException($"Invalid user status!");
			}
		}

		private async Task HandleNormalUserSignInAsync(ApplicationUser user, BaseResponse<UserSignedIn> response)
		{
			if (user.TwoFactorEnabled && (user.EmailConfirmed || user.PhoneNumberConfirmed))
			{
				if (!string.IsNullOrWhiteSpace(user.Email) && user.EmailConfirmed)
				{
					// TODO: Implement send OTP via Email
				}
				else
				{
					// TODO: Implement send OTP via SMS
				}
			}
			else
			{
				await CompleteSignInAsync(user, response);
			}
		}

		private async Task HandleAdminSignInAsync(ApplicationUser user, BaseResponse<UserSignedIn> response)
		{
			if (user.TwoFactorEnabled)
			{
				if (string.IsNullOrEmpty(user.SecretKey))
				{
					var qrCodeUrl = GenerateQRCodeUri(user);

					response.Successful(new UserSignedIn
					{
						TwoFactorEnabled = true,
						SecretKey = user.SecretKey,
						QRCodeUri = qrCodeUrl,
						UserName = user.UserName,
					});
				}
				else
				{
					response.Successful(new UserSignedIn
					{
						TwoFactorEnabled = true,
						SecretKey = user.SecretKey,
						UserName = user.UserName,
					});
				}
			}
			else
			{
				await CompleteSignInAsync(user, response);
			}
		}

		private async Task CompleteSignInAsync(ApplicationUser user, BaseResponse<UserSignedIn> response)
		{
			await UpdateRefreshToken(user);

			var roles = await GetAllRolesByUserId(user.Id);
			var claims = await GetAllClaimsByUserId(user.Id);

			var accessToken = GenerateAccessToken(user, roles, claims);

			response.Successful(new UserSignedIn
			{
				Id = user.Id,
				UserName = user.UserName,
				TimeZoneId = user.TimeZoneId,
				Currency = user.Currency,
				Language = user.Language,
				AccessToken = accessToken
			});
		}

		private async Task HandleFailedSignInAttemptAsync(ApplicationUser user)
		{
			if (user.Status == (int)UserStatusEnum.New || user.Status == (int)UserStatusEnum.Active)
			{
				user.AccessFailedCount = (user.AccessFailedCount ?? 0) + 1;

				if (user.AccessFailedCount >= _appSettings.Authentication.MaxAccessFailedCount)
				{
					user.Status = (int)UserStatusEnum.Inactive;
					user.LockoutEndDate_Utc = DateTime.UtcNow.AddDays(_appSettings.Authentication.NumberOfDaysLocked);
					user.LockedReason = "Exceeded failed login attempts";
				}

				await _mainUnitOfWork.SaveChangesAsync();
			}

			throw new UnauthorizedException($"Invalid password!");
		}
	}
}
