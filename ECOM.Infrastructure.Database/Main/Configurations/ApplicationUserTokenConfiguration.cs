using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class ApplicationUserTokenConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUserToken>
	{
		public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
		{
			builder.ToTable(nameof(ApplicationUserToken));

			builder.HasKey(x => new { x.UserId, x.Provider, x.TokenName });
			builder.Property(x => x.Provider).HasMaxLength(100).IsRequired();
			builder.Property(x => x.TokenName).HasMaxLength(100).IsRequired();
			builder.Property(x => x.TokenValue).HasMaxLength(int.MaxValue).IsRequired();
			builder.Property(x => x.TokenExpiredAt_Utc).IsRequired();

			builder.HasOne(x => x.User).WithMany(x => x.UserTokens).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
