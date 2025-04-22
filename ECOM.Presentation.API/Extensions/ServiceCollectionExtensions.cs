using ECOM.Shared.Library.Consts;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECOM.Presentation.API.Extensions
{
	/// <summary>
	/// Extension methods for registering authentication and CORS policies in the API layer.
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Registers JWT authentication with configuration from AppSettings section.
		/// </summary>
		/// <param name="services">The dependency injection container.</param>
		/// <param name="configuration">The application configuration.</param>
		/// <returns>The updated service collection.</returns>
		/// <exception cref="InvalidOperationException">Thrown if AppSettings are missing or invalid.</exception>
		public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()
						  ?? throw new InvalidOperationException("AppSettings section is missing or invalid.");

			// Add authentication with JWT bearer scheme
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

			// Add authorization middleware
			services.AddAuthorization();

			return services;
		}

		/// <summary>
		/// Registers a default CORS policy based on allowed origins in AppSettings.
		/// </summary>
		/// <param name="services">The dependency injection container.</param>
		/// <param name="configuration">The application configuration.</param>
		/// <returns>The updated service collection.</returns>
		/// <exception cref="Exception">Thrown if AppSettings are not properly initialized.</exception>
		public static IServiceCollection AddDefaultCorsPolicy(this IServiceCollection services, IConfiguration configuration)
		{
			var appSettings = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>()
				?? throw new Exception("Can not initialize AppSettings");

			var allowedOrigins = appSettings.Authentication.Cors.AllowedOrigins ?? [];

			// Register named CORS policy (ApplicationConstants.DefaultCors)
			services.AddCors(options =>
			{
				options.AddPolicy(ApplicationConstants.DefaultCors, builder =>
				{
					builder.WithOrigins(allowedOrigins)
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
				});
			});

			return services;
		}
	}
}
