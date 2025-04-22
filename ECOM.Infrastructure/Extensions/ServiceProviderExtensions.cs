using ECOM.Domain.Interfaces.Seeders;
using ECOM.Infrastructure.Database.Main;
using ECOM.Infrastructure.Database.MainLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECOM.Infrastructure.Extensions
{
	public static class ServiceProviderExtensions
	{
		public static async Task<IServiceProvider> InitializeDatabaseAsync(this IServiceProvider service)
		{
			await service.MigrateAsync<MainDbContext>();
			await service.MigrateAsync<MainLoggingDbContext>();
			await service.SeedDatabaseAsync();

			return service;
		}

		public static async Task<IServiceProvider> MigrateAsync<TContext>(this IServiceProvider service) where TContext : DbContext
		{
			using (var scope = service.CreateScope())
			{
				using var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
				await dbContext.Database.MigrateAsync();
			}
			return service;
		}

		public static async Task<IServiceProvider> SeedDatabaseAsync(this IServiceProvider service)
		{
			using (var scope = service.CreateScope())
			{
				var dbSeederModule = scope.ServiceProvider.GetRequiredService<IDbSeederModule>();
				if (dbSeederModule != null)
				{
					var seeders = scope.ServiceProvider.GetServices<IDbSeeder>();
					if (seeders != null && seeders.Any())
					{
						await dbSeederModule.InitializeAsync(seeders);
					}
				}
			}
			return service;
		}
	}
}
