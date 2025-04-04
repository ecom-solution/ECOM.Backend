using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace ECOM.Infrastructure.Logging.Targets
{
	public static class ConsoleLoggingTargetConfiguration
	{
		public static ILogger Initialize()
		{
			return new LoggerConfiguration()
			.WriteTo.Console(
				theme: AnsiConsoleTheme.Literate,
				outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}" +
								"Exception: {Exception}{NewLine}" +
								"Properties: {Properties}{NewLine}" +
								"Caller: {CallerMethod} | File: {CallerFileName} | Line: {CallerLineNumber}{NewLine}" +
								"IP: {IpAddress} | UserId: {UserId}{NewLine}"
			)
			.CreateLogger();
		}
	}
}
