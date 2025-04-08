using System.Text.Json;
using ECOM.App.DTOs.Common;
using ECOM.App.Exceptions;
using ECOM.Infrastructure.Logging.Interfaces;

namespace ECOM.Presentation.API.Middlewares
{
	public class GlobalExceptionMiddleware(RequestDelegate next, IEcomLogger logger, IHostEnvironment env)
	{
		private readonly RequestDelegate _next = next;
		private readonly IEcomLogger _logger = logger;
		private readonly IHostEnvironment _env = env;

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (HttpStatusException ex)
			{
				_logger.Error($"HttpStatusException: {ex.Message}", ex);
				await HandleHttpStatusExceptionAsync(context, ex, GetOptions());
			}
			catch (Exception ex)
			{
				_logger.Error("Unhandled Exception", ex);
				await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error", ex, GetOptions());
			}
		}

		private static async Task HandleHttpStatusExceptionAsync(HttpContext context, HttpStatusException ex, JsonSerializerOptions options)
		{
			var response = BaseResponse<object>.Failure(
				message: ex.Message,
				statusCode: ex.StatusCode
			);

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = ex.StatusCode;

			await context.Response.WriteAsJsonAsync(response, options);
		}

		private async Task HandleExceptionAsync(HttpContext context, int statusCode, string message, Exception exception, JsonSerializerOptions options)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;

			var errorResponse = new
			{
				statusCode,
				error = GetErrorName(statusCode),
				message,
				traceId = context.TraceIdentifier,
				path = context.Request.Path.Value,
				timestamp = DateTime.UtcNow.ToString("o"), // ISO 8601 format
				details = (_env.IsDevelopment() && statusCode == StatusCodes.Status500InternalServerError)
						  ? exception.ToString()
						  : null
			};
			var json = JsonSerializer.Serialize(errorResponse, options);
			await context.Response.WriteAsync(json);
		}

		private static JsonSerializerOptions GetOptions()
		{
			return new()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
			};
		}

		private static string GetErrorName(int statusCode)
		{
			return statusCode switch
			{
				StatusCodes.Status400BadRequest => "BadRequest",
				StatusCodes.Status401Unauthorized => "Unauthorized",
				StatusCodes.Status403Forbidden => "Forbidden",
				StatusCodes.Status404NotFound => "NotFound",
				StatusCodes.Status500InternalServerError => "InternalServerError",
				_ => "Error"
			};
		}
	}
}
