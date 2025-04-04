using ECOM.App.Exceptions;
using ECOM.Infrastructure.Logging.Interfaces;

namespace ECOM.Presentation.API.Middlewares
{
	public class GlobalExceptionMiddleware(RequestDelegate next, IEcomLogger logger)
	{
		private readonly RequestDelegate _next = next;
		private readonly IEcomLogger _logger = logger;

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (HttpStatusException ex)
			{
				_logger.Error($"HttpStatusException: {ex.Message}", ex);
				await HandleExceptionAsync(context, ex.StatusCode, ex.Message);
			}
			catch (Exception ex)
			{
				_logger.Error("Unhandled Exception", ex);
				await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error");
			}
		}

		private static Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = statusCode;

			var response = new
			{
				StatusCode = statusCode,
				Message = message
			};

			return context.Response.WriteAsJsonAsync(response);
		}
	}
}
