using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class ApplicationUserConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.ToTable(nameof(ApplicationUser));

			builder.HasKey(x => x.Id);

			builder.Property(x => x.UserName).HasMaxLength(100).IsRequired();
			builder.Property(x => x.NormalizedUserName).HasMaxLength(100).IsRequired();

			builder.Property(x => x.FullName).HasMaxLength(100).IsRequired();
			builder.Property(x => x.NormalizedFullName).HasMaxLength(100).IsRequired();

			builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
			builder.Property(x => x.NormalizedEmail).HasMaxLength(100).IsRequired();
			builder.Property(x => x.EmailConfirmed).HasDefaultValue(false).IsRequired();

			builder.Property(x => x.PhoneNumber).HasMaxLength(15).IsRequired();
			builder.Property(x => x.PhoneNumberConfirmed).HasDefaultValue(false).IsRequired();

			builder.Property(x => x.SecretKey).HasMaxLength(100).IsRequired();
			builder.Property(x => x.PasswordHash).HasMaxLength(int.MaxValue).IsRequired();
			builder.Property(x => x.SecurityStamp).HasMaxLength(100).IsRequired();
			builder.Property(x => x.ConcurrencyStamp).HasMaxLength(100).IsRequired();

			builder.Property(x => x.TimeZoneId).HasMaxLength(100).IsUnicode(false).IsRequired();
			builder.Property(x => x.Currency).HasMaxLength(3).IsUnicode(false).IsRequired();
			builder.Property(x => x.Language).HasMaxLength(2).IsUnicode(false).IsRequired();

			builder.Property(x => x.TwoFactorEnabled).HasDefaultValue(false).IsRequired();
			builder.Property(x => x.Status).IsRequired();

			builder.Property(x => x.ConnectionStatus).IsRequired().HasDefaultValue(0); // 0: Offline

			builder.Property(x => x.LockedReason).HasMaxLength(250).IsRequired();

			builder.HasOne(x => x.Avartar)
				.WithMany(x => x.Users)
				.HasForeignKey(x => x.AvatarId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.UserRoles)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.UserClaims)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.UserLogins)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.UserTokens)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
