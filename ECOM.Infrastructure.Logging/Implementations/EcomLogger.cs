using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Shared.Utilities.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ECOM.Infrastructure.Logging.Implementations
{
	public class EcomLogger(
		ILogger logger,
		IOptions<AppSettings> appSettings,
		IHttpContextAccessor httpContextAccessor) : IEcomLogger
	{
		private readonly ILogger _logger = logger;
		private readonly AppSettings _appSettings = appSettings.Value;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public void Debug(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			Log(LogEventLevel.Debug, message, exception, callerMethod, callerFileName, callerLineNumber);
		}

		public void Error(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			Log(LogEventLevel.Error, message, exception, callerMethod, callerFileName, callerLineNumber);
		}

		public void Fatal(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			Log(LogEventLevel.Fatal, message, exception, callerMethod, callerFileName, callerLineNumber);
		}

		public void Information(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			Log(LogEventLevel.Information, message, exception, callerMethod, callerFileName, callerLineNumber);
		}

		public void Verbose(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			Log(LogEventLevel.Verbose, message, exception, callerMethod, callerFileName, callerLineNumber);
		}

		public void Warning(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			Log(LogEventLevel.Warning, message, exception, callerMethod, callerFileName, callerLineNumber);
		}

		private void Log(LogEventLevel level, string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			// Push properties into the log context for automatic inclusion
			using (LogContext.PushProperty("CallerMethod", callerMethod))
			using (LogContext.PushProperty("CallerFileName", callerFileName))
			using (LogContext.PushProperty("CallerLineNumber", callerLineNumber))
			using (LogContext.PushProperty("CreatedAt_Utc", DateTime.UtcNow))
			using (LogContext.PushProperty("IpAddress", GetIpAddress()))
			using (LogContext.PushProperty("UserId", GetUserId()))
			{
				switch (level)
				{
					case LogEventLevel.Verbose:
						_logger.Verbose(message, exception);
						break;
					case LogEventLevel.Debug:
						_logger.Debug(message, exception);
						break;
					case LogEventLevel.Information:
						_logger.Information(message, exception);
						break;
					case LogEventLevel.Warning:
						_logger.Warning(message, exception);
						break;
					case LogEventLevel.Error:
						_logger.Error(message, exception);
						break;
					case LogEventLevel.Fatal:
						_logger.Fatal(message, exception);
						break;
					default:
						break;
				}
			}
		}

		private string GetIpAddress()
		{
			var context = _httpContextAccessor.HttpContext;
			if (context?.Connection?.RemoteIpAddress != null)
			{
				return context.Connection.RemoteIpAddress.ToString();
			}
			return "Unknown";
		}

		private Guid? GetUserId()
		{
			var context = _httpContextAccessor.HttpContext;
			if (context?.User?.Identity?.IsAuthenticated == true)
			{
				var userIdClaim = context.User.FindFirst("sub") ?? context.User.FindFirst(ClaimTypes.NameIdentifier);
				if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
				{
					return userId;
				}
			}
			return Guid.Empty;
		}
	}
}
