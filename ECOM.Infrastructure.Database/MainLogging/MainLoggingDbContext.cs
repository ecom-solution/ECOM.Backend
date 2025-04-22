using ECOM.Domain.Entities.MainLogging;
using Microsoft.EntityFrameworkCore;

namespace ECOM.Infrastructure.Database.MainLogging
{
    public class MainLoggingDbContext(DbContextOptions<MainLoggingDbContext> options) : DbContext(options)
	{
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<TransactionLog> TransactionLog { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainLoggingDbContext).Assembly);

			base.OnModelCreating(modelBuilder);
		}
	}
}
