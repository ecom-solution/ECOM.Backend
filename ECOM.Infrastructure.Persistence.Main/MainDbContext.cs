using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Infrastructure.Persistence.Main
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
