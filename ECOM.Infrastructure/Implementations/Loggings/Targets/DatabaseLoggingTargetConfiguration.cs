using Serilog;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace ECOM.Infrastructure.Implementations.Loggings.Targets
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
				AdditionalColumns =
				[
					new(nameof(Domain.Entities.MainLogging.Log.CallerMethod), SqlDbType.NVarChar, true, 256),
					new(nameof(Domain.Entities.MainLogging.Log.CallerFileName), SqlDbType.NVarChar, true, 512),
					new(nameof(Domain.Entities.MainLogging.Log.CallerLineNumber), SqlDbType.Int, true),
					new(nameof(Domain.Entities.MainLogging.Log.IpAddress), SqlDbType.NVarChar, true, 45),
					new(nameof(Domain.Entities.MainLogging.Log.UserId), SqlDbType.UniqueIdentifier, true)
				]
			};

			return columnOptions;
		}
	}
}
