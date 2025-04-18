using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ECOM.Infrastructure.Database.MainLogging
{
	public class MainLoggingDbContextFactory : IDesignTimeDbContextFactory<MainLoggingDbContext>
	{
		public MainLoggingDbContext CreateDbContext(string[] args)
		{
			var configurationBuilder = new ConfigurationBuilder()
											.SetBasePath(Directory.GetCurrentDirectory())
											.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
											.Build();

			var connectionString = configurationBuilder.GetConnectionString(nameof(MainLoggingDbContext)) ?? string.Empty;


			var optionsBuilder = new DbContextOptionsBuilder<MainLoggingDbContext>();

			optionsBuilder.UseSqlServer(connectionString);

			return new MainLoggingDbContext(optionsBuilder.Options);
		}
	}
}
