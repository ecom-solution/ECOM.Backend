using ECOM.Shared.Library.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECOM.Presentation.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()
						  ?? throw new InvalidOperationException("AppSettings section is missing or invalid.");

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = appSettings.Authentication.Jwt.ValidIssuer,
					ValidAudience = appSettings.Authentication.Jwt.ValidAudience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Authentication.Jwt.SecretKey))
				};
			});

			services.AddAuthorization();

			return services;
		}
	}
}
