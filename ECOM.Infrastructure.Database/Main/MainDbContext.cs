using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Infrastructure.Database.Main
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
	{
		public virtual DbSet<ApplicationClaim> ApplicationClaim { get; set; }
		public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
		public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaim { get; set; }
		public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
		public virtual DbSet<ApplicationUserClaim> ApplicationUserClaim { get; set; }
		public virtual DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
		public virtual DbSet<ApplicationUserLogin> ApplicationUserLogin { get; set; }
		public virtual DbSet<ApplicationUserToken> ApplicationUserToken { get; set; }

		public virtual DbSet<FileEntity> FileEntity { get; set; }

		public virtual DbSet<Language> Language { get; set; }
		public virtual DbSet<LanguageKey> LanguageKey { get; set; }
		public virtual DbSet<LanguageTranslation> LanguageTranslation { get; set; }
		public virtual DbSet<LanguageTranslationEntity> LanguageTranslationEntity { get; set; }

		public virtual DbSet<Notification> Notification { get; set; }
		public virtual DbSet<NotificationLink> NotificationLink { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var applyMethod = typeof(ModelBuilder).GetMethods()
												  .First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration)
														   && m.GetParameters().First().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

			var configurations = typeof(MainDbContext).Assembly
				.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract)
				.Where(t => typeof(MainConfiguration).IsAssignableFrom(t))
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
