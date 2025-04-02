using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Infrastructure.Persistence.Main
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
	{
		public virtual DbSet<ApplicationClaim> ApplicationClaim { get; set; }
		public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
		public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaim { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
