using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Persistence.Main.Configurations
{
	public class ApplicationClaimConfiguration : IEntityTypeConfiguration<ApplicationClaim>
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
