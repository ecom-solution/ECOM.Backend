using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class ApplicationUserClaimConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUserClaim>
	{
		public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
		{
			builder.ToTable(nameof(ApplicationUserClaim));

			builder.HasKey(x => new { x.UserId, x.ClaimId });

			builder.HasOne(x => x.User).WithMany(x => x.UserClaims).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x => x.Claim).WithMany(x => x.UserClaims).HasForeignKey(x => x.ClaimId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
