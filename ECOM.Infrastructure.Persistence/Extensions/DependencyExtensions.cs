using ECOM.Infrastructure.Persistence.Main;
using ECOM.Infrastructure.Persistence.MainLogging;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECOM.Infrastructure.Persistence.Extensions
{
	public static class DependencyExtensions
	{
		public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<MainDbContext>(configuration);
			services.AddDbContext<MainLoggingDbContext>(configuration);

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

		public static async Task<IServiceProvider> MigrateAsync<TContext>(this IServiceProvider service) where TContext : DbContext
		{
			using (var scope = service.CreateScope())
			{
				using var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
				await dbContext.Database.MigrateAsync();
			}
			return service;
		}
	}
}
