using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Infrastructure.Database.Main
{
	public class MainDbContextFactory : IDesignTimeDbContextFactory<MainDbContext>
	{
		public MainDbContext CreateDbContext(string[] args)
		{
			var configurationBuilder = new ConfigurationBuilder()
											.SetBasePath(Directory.GetCurrentDirectory())
										    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
											.Build();

			var connectionString = configurationBuilder.GetConnectionString(nameof(MainDbContext)) ?? string.Empty;


			var optionsBuilder = new DbContextOptionsBuilder<MainDbContext>();

			optionsBuilder.UseSqlServer(connectionString);

			return new MainDbContext(optionsBuilder.Options);
		}
	}
}
