using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace ECOM.Presentation.API.Middlewares
{
	public class JwtValidationMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
	{
		private readonly RequestDelegate _next = next;
		private readonly AppSettings _appSettings = appSettings.Value;

		public async Task InvokeAsync(HttpContext context)
		{
			var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

			if (!string.IsNullOrEmpty(token))
			{
				var validationResult = ValidateJwtToken(token);

				if (!validationResult)
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					context.Response.ContentType = "application/json";

					var errorResponse = new
					{
						statusCode = 401,
						error = "Unauthorized",
						message = "Invalid or expired JWT Token"
					};

					var json = JsonSerializer.Serialize(errorResponse);
					await context.Response.WriteAsync(json);

					return;
				}
			}

			await _next(context);
		}

		private bool ValidateJwtToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_appSettings.Authentication.Jwt.SecretKey);

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = true,
					ValidIssuer = _appSettings.Authentication.Jwt.ValidIssuer,
					ValidateAudience = true,
					ValidAudience = _appSettings.Authentication.Jwt.ValidAudience,
					ValidateLifetime = true,
				}, out SecurityToken validatedToken);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
