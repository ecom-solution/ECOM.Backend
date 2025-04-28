using ECOM.Domain.Entities.MainLogging;
using ECOM.Infrastructure.Database.MainLogging.Common;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Infrastructure.Database.MainLogging
{
	public class MainLoggingDbContext(DbContextOptions<MainLoggingDbContext> options) : DbContext(options)
	{
		public virtual DbSet<Log> Logs { get; set; }
		public virtual DbSet<TransactionLog> TransactionLog { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var applyMethod = typeof(ModelBuilder).GetMethods()
												  .First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration)
														   && m.GetParameters().First().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

			var configurations = typeof(MainLoggingDbContext).Assembly
				.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract)
				.Where(t => typeof(MainLoggingConfiguration).IsAssignableFrom(t))
				.Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
				.ToList();

			foreach (var configType in configurations)
			{
				var interfaceType = configType.GetInterfaces()
					.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

				var entityType = interfaceType.GetGenericArguments().First();
				var configInstance = Activator.CreateInstance(configType);
				var genericMethod = applyMethod.MakeGenericMethod(entityType);
				genericMethod.Invoke(modelBuilder, [configInstance]);
			}

			base.OnModelCreating(modelBuilder);
		}
	}
}
