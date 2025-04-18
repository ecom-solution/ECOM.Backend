using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class ApplicationClaimConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationClaim>
	{
		public void Configure(EntityTypeBuilder<ApplicationClaim> builder)
		{
			builder.ToTable(nameof(ApplicationClaim));

			builder.HasKey(x => x.Id);
			builder.Property(x => x.ClaimType).HasMaxLength(250).IsRequired();
			builder.Property(x => x.ClaimValue).HasMaxLength(250).IsRequired();
			builder.Property(x => x.Description).HasMaxLength(500);

			builder.HasMany(x => x.RoleClaims).WithOne(x => x.Claim).HasForeignKey(x => x.ClaimId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
