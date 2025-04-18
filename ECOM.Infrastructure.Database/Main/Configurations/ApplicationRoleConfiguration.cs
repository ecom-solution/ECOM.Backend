using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class ApplicationRoleConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			builder.ToTable(nameof(ApplicationRole));

			builder.HasKey(x => x.Id);
			builder.Property(x => x.Name).HasMaxLength(250).IsRequired();
			builder.Property(x => x.Description).HasMaxLength(500);

			builder.HasMany(x => x.RoleClaims).WithOne(x => x.Role).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
