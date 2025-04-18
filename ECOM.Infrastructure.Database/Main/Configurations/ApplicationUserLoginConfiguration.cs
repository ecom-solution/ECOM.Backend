using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class ApplicationUserLoginConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUserLogin>
	{
		public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
		{
			builder.ToTable(nameof(ApplicationUserLogin));

			builder.HasKey(x => new { x.Provider, x.ProviderKey });
			builder.Property(x => x.Provider).HasMaxLength(100).IsRequired();
			builder.Property(x => x.ProviderKey).HasMaxLength(100).IsRequired();

			builder.HasOne(x => x.User).WithMany(x => x.UserLogins).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
