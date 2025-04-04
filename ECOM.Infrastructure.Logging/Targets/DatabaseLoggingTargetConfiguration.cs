using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace ECOM.Infrastructure.Logging.Targets
{
	public class DatabaseLoggingTargetConfiguration
	{
		public static ILogger Initialize(IConfiguration configuration)
		{
			return new LoggerConfiguration()
						.ReadFrom.Configuration(configuration)
						.Enrich.FromLogContext()
						.WriteTo.MSSqlServer(
							connectionString: configuration.GetConnectionString("MainLoggingDbContext"),
							sinkOptions: new MSSqlServerSinkOptions { TableName = "Log", AutoCreateSqlTable = false },
							columnOptions: GetSqlColumnOptions()
						)
						.CreateLogger();
		}

		private static ColumnOptions GetSqlColumnOptions()
		{
			var columnOptions = new ColumnOptions
			{
				// Add custom columns
				AdditionalColumns =
				[
					new("CallerMethod", SqlDbType.NVarChar, true, 256),
					new("CallerFileName", SqlDbType.NVarChar, true, 512),
					new("CallerLineNumber", SqlDbType.Int, true),
					new("IpAddress", SqlDbType.NVarChar, true, 45),
					new("UserId", SqlDbType.UniqueIdentifier, true)
				]
			};

			return columnOptions;
		}
	}
}
