using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Logging.Targets;
using ECOM.Shared.Utilities.Settings;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ECOM.Infrastructure.Logging.Implementations
{
	public class EcomLogger : IEcomLogger
	{
		private readonly ILogger _logger;
		private readonly AppSettings _appSettings;

		public EcomLogger(IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;
			_logger = ConsoleLoggingTargetConfiguration.Configure();
		}

		public void Debug(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			throw new NotImplementedException();
		}

		public void Error(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			throw new NotImplementedException();
		}

		public void Fatal(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			throw new NotImplementedException();
		}

		public void Information(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			throw new NotImplementedException();
		}

		public void Verbose(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			throw new NotImplementedException();
		}

		public void Warning(string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
		{
			throw new NotImplementedException();
		}

		private void Log(LogEventLevel level,string message, Exception? exception = null, [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFileName = "", [CallerLineNumber] int callerLineNumber = 0)
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
						break;
					case LogEventLevel.Debug:
						break;
					case LogEventLevel.Information:
						break;
					case LogEventLevel.Warning:
						break;
					case LogEventLevel.Error:
						break;
					case LogEventLevel.Fatal:
						break;
					default:
						break;
				}
			}
		}

		private string GetIpAddress()
		{
			// Example method to get IP address
			return "127.0.0.1";
		}

		private Guid? GetUserId()
		{
			// Example method to get user ID
			return Guid.NewGuid();  // Replace with actual user ID logic
		}
	}
}
