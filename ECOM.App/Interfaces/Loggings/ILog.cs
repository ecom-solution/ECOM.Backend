using System.Runtime.CompilerServices;

namespace ECOM.App.Interfaces.Loggings
{
	public interface ILog
	{
		/// <summary>
		/// Logs a Verbose-level message. This is the most detailed log level, used for deep debugging.
		/// </summary>
		/// <param name="message">The log message.</param>
		/// <param name="exception">An optional exception.</param>
		/// <param name="callerMethod">The name of the method calling the log (automatically retrieved).</param>
		/// <param name="callerFileName">The name of the file containing the calling method (automatically retrieved).</param>
		/// <param name="callerLineNumber">The line number in the file where the log is called (automatically retrieved).</param>
		void Verbose(string message, Exception? exception = null,
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFileName = "",
			[CallerLineNumber] int callerLineNumber = 0);

		/// <summary>
		/// Logs a Debug-level message. Used during development to track the execution flow.
		/// </summary>
		/// <param name="message">The log message.</param>
		/// <param name="exception">An optional exception.</param>
		/// <param name="callerMethod">The name of the method calling the log (automatically retrieved).</param>
		/// <param name="callerFileName">The name of the file containing the calling method (automatically retrieved).</param>
		/// <param name="callerLineNumber">The line number in the file where the log is called (automatically retrieved).</param>
		void Debug(string message, Exception? exception = null,
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFileName = "",
			[CallerLineNumber] int callerLineNumber = 0);

		/// <summary>
		/// Logs an Info-level message. Used to record important system events.
		/// </summary>
		/// <param name="message">The log message.</param>
		/// <param name="exception">An optional exception.</param>
		/// <param name="callerMethod">The name of the method calling the log (automatically retrieved).</param>
		/// <param name="callerFileName">The name of the file containing the calling method (automatically retrieved).</param>
		/// <param name="callerLineNumber">The line number in the file where the log is called (automatically retrieved).</param>
		void Information(string message, Exception? exception = null,
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFileName = "",
			[CallerLineNumber] int callerLineNumber = 0);

		/// <summary>
		/// Logs a Warn-level message. Used to indicate potential issues or warnings.
		/// </summary>
		/// <param name="message">The log message.</param>
		/// <param name="exception">An optional exception.</param>
		/// <param name="callerMethod">The name of the method calling the log (automatically retrieved).</param>
		/// <param name="callerFileName">The name of the file containing the calling method (automatically retrieved).</param>
		/// <param name="callerLineNumber">The line number in the file where the log is called (automatically retrieved).</param>
		void Warning(string message, Exception? exception = null,
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFileName = "",
			[CallerLineNumber] int callerLineNumber = 0);

		/// <summary>
		/// Logs an Error-level message. Indicates a failure, but the application can still continue running.
		/// </summary>
		/// <param name="message">The log message.</param>
		/// <param name="exception">An optional exception.</param>
		/// <param name="callerMethod">The name of the method calling the log (automatically retrieved).</param>
		/// <param name="callerFileName">The name of the file containing the calling method (automatically retrieved).</param>
		/// <param name="callerLineNumber">The line number in the file where the log is called (automatically retrieved).</param>
		void Error(string message, Exception? exception = null,
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFileName = "",
			[CallerLineNumber] int callerLineNumber = 0);

		/// <summary>
		/// Logs a Fatal-level message. Indicates a severe failure that may cause the system to stop functioning.
		/// </summary>
		/// <param name="message">The log message.</param>
		/// <param name="exception">An optional exception.</param>
		/// <param name="callerMethod">The name of the method calling the log (automatically retrieved).</param>
		/// <param name="callerFileName">The name of the file containing the calling method (automatically retrieved).</param>
		/// <param name="callerLineNumber">The line number in the file where the log is called (automatically retrieved).</param>
		void Fatal(string message, Exception? exception = null,
			[CallerMemberName] string callerMethod = "",
			[CallerFilePath] string callerFileName = "",
			[CallerLineNumber] int callerLineNumber = 0);
	}
}
