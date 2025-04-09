using ECOM.Domain.Interfaces.Repositories;
using ECOM.Domain.Interfaces.Seeders;
using ECOM.Infrastructure.Persistence.Implementations.Repositories;
using ECOM.Infrastructure.Persistence.Implementations.Seeders;
using ECOM.Infrastructure.Persistence.Main;
using ECOM.Infrastructure.Persistence.MainLogging;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ECOM.Infrastructure.Persistence.Extensions
{
	public static class DependencyExtensions
	{
		public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<MainDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString(nameof(MainDbContext))));

			services.AddDbContext<MainLoggingDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString(nameof(MainLoggingDbContext))));

			services.AddScoped<IUnitOfWork<MainDbContext>, UnitOfWork<MainDbContext>>();
			services.AddScoped<IUnitOfWork<MainLoggingDbContext>, UnitOfWork<MainLoggingDbContext>>();

			services.AddDbSeeders();
			services.AddScoped<IDbSeederModule, DbSeederModule>();

			return services;
		}

		public static IServiceCollection AddDbContext<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
		{
			var contextName = typeof(TContext).Name;
			var contextNamespace = typeof(TContext).Namespace;

			services.AddDbContext<TContext>(options => options.UseSqlServer(configuration.GetConnectionString(contextName),
				x => x.MigrationsAssembly(contextNamespace)));

			return services;
		}

		public static async Task<IServiceProvider> MigrateAsync(this IServiceProvider service)
		{
			await service.MigrateAsync<MainDbContext>();
			await service.MigrateAsync<MainLoggingDbContext>();

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

		public static IServiceCollection AddDbSeeders(this IServiceCollection services)
		{
			var seederType = typeof(IDbSeeder);

			var seederTypes = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface && seederType.IsAssignableFrom(t));

			foreach (var type in seederTypes)
			{
				services.AddTransient(seederType, type);
			}

			return services;
		}

		public static IServiceCollection AddDbSeederModules(this IServiceCollection services)
		{
			var seederType = typeof(IDbSeederModule);

			var seederTypes = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(t => !t.IsAbstract && !t.IsInterface && seederType.IsAssignableFrom(t));

			foreach (var type in seederTypes)
			{
				services.AddTransient(seederType, type);
			}

			return services;
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
