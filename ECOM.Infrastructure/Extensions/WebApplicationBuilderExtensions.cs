using ECOM.App.Interfaces.Loggings;
using ECOM.Infrastructure.Implementations.Loggings;
using ECOM.Infrastructure.Implementations.Loggings.Targets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ECOM.Infrastructure.Extensions
{
	public static class WebApplicationBuilderExtensions
	{
		public static void AddLogging(this WebApplicationBuilder builder, IConfiguration configuration)
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
			builder.Services.AddSingleton<ILog, ApplicationLogger>();
		}
	}
}
