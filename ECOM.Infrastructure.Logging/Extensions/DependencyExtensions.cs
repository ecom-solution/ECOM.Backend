using ECOM.Infrastructure.Logging.Implementations;
using ECOM.Infrastructure.Logging.Interfaces;
using ECOM.Infrastructure.Logging.Targets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ECOM.Infrastructure.Logging.Extensions
{
	public static class DependencyExtensions
	{
		public static void AddECOMLogging(this WebApplicationBuilder builder, IConfiguration configuration)
		{
			if (builder.Environment.EnvironmentName == "Development")
			{
				Log.Logger = ConsoleLoggingTargetConfiguration.Initialize();
			}
			else
			{
				Log.Logger = DatabaseLoggingTargetConfiguration.Initialize(configuration);
			}
			builder.Host.UseSerilog();
			builder.Services.AddSingleton(sp => Log.Logger);
			builder.Services.AddSingleton<IEcomLogger, EcomLogger>();
		}
	}
}
