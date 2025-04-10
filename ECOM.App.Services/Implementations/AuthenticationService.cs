using AutoMapper;
using ECOM.App.DTOs.Common;
using ECOM.App.DTOs.Modules.Authentication.Users;
using ECOM.App.Exceptions.HttpStatus;
using ECOM.App.Services.Common;
using ECOM.App.Services.Interfaces;
using ECOM.Domain.Entities.Main;
using ECOM.Domain.Interfaces.Repositories;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Infrastructure.Persistence.MainLogging;
using ECOM.Shared.Utilities.Constants;
using ECOM.Shared.Utilities.Enums;
using ECOM.Shared.Utilities.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECOM.App.Services.Implementations
{
	internal class AuthenticationService(
		IMapper mapper, 
		IEcomLogger logger, 
		IOptions<AppSettings> appSettings, 
		IUnitOfWork<MainDbContext> mainUnitOfWork, 
		IUnitOfWork<MainLoggingDbContext> mainLoggingUnitOfWork) : BaseService(mapper, logger, appSettings, mainUnitOfWork, mainLoggingUnitOfWork), IAuthenticationService
	{
		public async Task<BaseResponse<UserSignedIn>> SignUpAsync(BaseRequest<UserSignUp> request)
		{
			using var transaction = await _mainUnitOfWork.GetContext().Database.BeginTransactionAsync();
			try
			{
				var userSignUp = request.Model;
				if (string.IsNullOrWhiteSpace(userSignUp.UserName) || string.IsNullOrWhiteSpace(userSignUp.Password))
					throw new BadRequestException($"UserName and Password are required.");

				var existingUser = await _mainUnitOfWork.Repository<ApplicationUser>().Where(x => string.Equals(x.UserName, userSignUp.UserName, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();
				if (existingUser != null)
					throw new BadRequestException($"UserName {userSignUp.UserName} is already registered.");

				var user = new ApplicationUser()
				{
					UserName = userSignUp.UserName.Trim().ToLower(),
					NormalizedUserName = userSignUp.UserName.Trim().ToLower(),
					PasswordHash = HashPassword(userSignUp.Password),
					SecretKey = Guid.NewGuid().ToString(),
					SecurityStamp = Guid.NewGuid().ToString(),
					ConcurrencyStamp = Guid.NewGuid().ToString(),
					TimeZoneId = string.IsNullOrWhiteSpace(userSignUp.TimeZoneId) ? ApplicationConstants.DefaultTimeZoneId : userSignUp.TimeZoneId,
					Currency = string.IsNullOrWhiteSpace(userSignUp.Currency) ? ApplicationConstants.DefaultCurrency : userSignUp.Currency,
					Language = string.IsNullOrWhiteSpace(userSignUp.Language) ? ApplicationConstants.DefaultLanguage : userSignUp.Language,
				};

				await _mainUnitOfWork.Repository<ApplicationUser>().InsertAsync(user);
				await _mainUnitOfWork.SaveChangesAsync(isPartOfTransaction: true);

				//Add user to Admin role // test
				var defaultRole = await _mainUnitOfWork.Repository<ApplicationRole>().FirstOrDefaultAsync(x => x.Name == AuthenticationConstants.Role.Admin) ?? throw new NotFoundException($"Default role not found!");

				var userRole = new ApplicationUserRole(user.Id, defaultRole.Id);
				await _mainUnitOfWork.Repository<ApplicationUserRole>().InsertAsync(userRole);
				await _mainUnitOfWork.SaveChangesAsync(isPartOfTransaction: true);

				var refreshToken = GenerateRefreshToken(user.Id);
				await _mainUnitOfWork.Repository<ApplicationUserToken>().InsertAsync(refreshToken);
				await _mainUnitOfWork.SaveChangesAsync(isPartOfTransaction: true);

				await transaction.CommitAsync();

				return BaseResponse<UserSignedIn>.Success(new UserSignedIn()
				{
					Id = user.Id,
					UserName = user.UserName,
					TimeZoneId = user.TimeZoneId,
					Currency = user.Currency,
					Language = user.Language,
					AccessToken = GenerateAccessToken(user, [defaultRole], [])
				});
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public async Task<BaseResponse<UserSignedIn>> SignInAsync(BaseRequest<UserSignIn> request)
		{
			using var transaction = await _mainUnitOfWork.GetContext().Database.BeginTransactionAsync();
			try
			{
				var userSignIn = request.Model;
				var user = await _mainUnitOfWork.Repository<ApplicationUser>()
											    .Where(x => string.Equals(x.UserName, userSignIn.UserName, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync()
											     ?? throw new NotFoundException($"User {userSignIn.UserName} not found!");

				if (!VerifyPassword(userSignIn.Password, user.PasswordHash))
				{
					HandleFailedLoginAttempt(user);

					_mainUnitOfWork.Repository<ApplicationUser>().Update(user);
					await _mainUnitOfWork.SaveChangesAsync(isPartOfTransaction: true);
					await transaction.CommitAsync();

					return BaseResponse<UserSignedIn>.Failure($"Password is incorrect!", statusCode: StatusCodes.Status401Unauthorized);
				}
				else
				{
					//re-migration
					throw new Exception();
				}
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public async Task<BaseResponse<UserSignedIn>> AdminSignInAsync(BaseRequest<UserSignIn> request)
		{
			using var transaction = await _mainUnitOfWork.GetContext().Database.BeginTransactionAsync();
			try
			{
				throw new Exception();
			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

		public Task<BaseResponse<UserSignedOut>> SignOutAsync(BaseRequest<UserSignedOut> request)
		{
			throw new NotImplementedException();
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

		private ApplicationUserToken GenerateRefreshToken(Guid userId)
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			return new ApplicationUserToken()
			{
				UserId = userId,
				Provider = ApplicationConstants.AppName,
				TokenName = "RefressToken",
				TokenValue = Convert.ToBase64String(randomNumber),
				TokenExpiredAt_Utc = DateTime.UtcNow.AddDays(_appSettings.Authentication.Jwt.RefreshTokenValidityInDays)
			};
		}

		private static string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}

		private static bool VerifyPassword(string password, string hashedPassword)
		{
			return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
		}

		private void HandleFailedLoginAttempt(ApplicationUser user)
		{
			if (user.Status == (int)UserStatus.Active || user.Status == (int)UserStatus.New)
			{
				if (user.AccessFailedCount >= _appSettings.Authentication.MaxAccessFailedCount)
				{
					user.AccessFailedCount = 0;
					user.Status = (int)UserStatus.Inactive;
					user.LockoutEndDate_Utc = DateTime.UtcNow.AddDays(_appSettings.Authentication.NumberOfDaysLocked);
					user.LockedReason = $"User is locked due to failed login attempts!";
				}
				else
				{
					user.AccessFailedCount++;
				}
			}
		}

		private async Task<List<ApplicationRole>> GetAllRolesByUserId(Guid userId)
		{
			var userRoles = await _mainUnitOfWork.Repository<ApplicationUserRole>().Where(ur => ur.UserId == userId)
												 .Join(_mainUnitOfWork.Repository<ApplicationRole>().Query(), ur => ur.RoleId, r => r.Id, (ur, r) => r)
												 .ToListAsync() ?? [];
			return userRoles;
		}

		private async Task<List<ApplicationClaim>> GetAllClaimsByUserId(Guid userId)
		{
			var userClaims = await _mainUnitOfWork.Repository<ApplicationUserClaim>()
								.Where(uc => uc.UserId == userId)
								.Join(_mainUnitOfWork.Repository<ApplicationClaim>().Query(), uc => uc.ClaimId, c => c.Id, (uc, c) => c)
								.Union(
									_mainUnitOfWork.Repository<ApplicationUserRole>()
									.Where(ur => ur.UserId == userId)
									.Join(_mainUnitOfWork.Repository<ApplicationRoleClaim>().Query(), ur => ur.RoleId, rc => rc.RoleId, (ur, rc) => rc)
									.Join(_mainUnitOfWork.Repository<ApplicationClaim>().Query(), rc => rc.ClaimId, c => c.Id, (rc, c) => c)
								)
								.Distinct()
								.ToListAsync() ?? [];
			return userClaims;
		}

	}
}
