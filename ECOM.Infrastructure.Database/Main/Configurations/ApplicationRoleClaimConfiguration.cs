using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class ApplicationRoleClaimConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationRoleClaim>
	{
		public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
		{
			builder.ToTable(nameof(ApplicationRoleClaim));

			builder.HasKey(x => new { x.RoleId, x.ClaimId });

			builder.HasOne(x => x.Role).WithMany(x => x.RoleClaims).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x => x.Claim).WithMany(x => x.RoleClaims).HasForeignKey(x => x.ClaimId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
