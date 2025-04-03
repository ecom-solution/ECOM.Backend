using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Persistence.Main.Configurations
{
	public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
		{
			builder.ToTable(nameof(ApplicationUserRole));

			builder.HasKey(x => new { x.UserId, x.RoleId });

			builder.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
