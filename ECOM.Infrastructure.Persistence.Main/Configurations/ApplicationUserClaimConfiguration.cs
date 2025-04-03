using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Persistence.Main.Configurations
{
	public class ApplicationUserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
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
