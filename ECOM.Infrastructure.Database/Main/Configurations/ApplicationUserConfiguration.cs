using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationUser"/> entity, defining its mapping to the database table,
    /// properties, and relationships with other entities within the ECOM domain.
    /// This class implements the <see cref="IEntityTypeConfiguration{ApplicationUser}"/> interface.
    /// </summary>
    public class ApplicationUserConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUser>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationUser"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable(nameof(ApplicationUser)); // Maps the entity to a table named "ApplicationUser"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key

            builder.Property(x => x.UserName).HasMaxLength(100).IsRequired(); // Configures the "UserName" property: maximum length 100 and required
            builder.Property(x => x.NormalizedUserName).HasMaxLength(100).IsRequired(); // Configures the "NormalizedUserName" property: maximum length 100 and required

            builder.Property(x => x.FullName).HasMaxLength(100).IsRequired(); // Configures the "FullName" property: maximum length 100 and required
            builder.Property(x => x.NormalizedFullName).HasMaxLength(100).IsRequired(); // Configures the "NormalizedFullName" property: maximum length 100 and required

            builder.Property(x => x.Email).HasMaxLength(100).IsRequired(); // Configures the "Email" property: maximum length 100 and required
            builder.Property(x => x.NormalizedEmail).HasMaxLength(100).IsRequired(); // Configures the "NormalizedEmail" property: maximum length 100 and required
            builder.Property(x => x.EmailConfirmed).HasDefaultValue(false).IsRequired(); // Configures the "EmailConfirmed" property: default value false and required

            builder.Property(x => x.PhoneNumber).HasMaxLength(15).IsRequired(); // Configures the "PhoneNumber" property: maximum length 15 and required
            builder.Property(x => x.PhoneNumberConfirmed).HasDefaultValue(false).IsRequired(); // Configures the "PhoneNumberConfirmed" property: default value false and required

            builder.Property(x => x.SecretKey).HasMaxLength(100).IsRequired(); // Configures the "SecretKey" property: maximum length 100 and required
            builder.Property(x => x.PasswordHash).HasMaxLength(int.MaxValue).IsRequired(); // Configures the "PasswordHash" property: maximum length unlimited and required
            builder.Property(x => x.SecurityStamp).HasMaxLength(100).IsRequired(); // Configures the "SecurityStamp" property: maximum length 100 and required
            builder.Property(x => x.ConcurrencyStamp).HasMaxLength(100).IsRequired(); // Configures the "ConcurrencyStamp" property: maximum length 100 and required

            builder.Property(x => x.TimeZoneId).HasMaxLength(100).IsUnicode(false).IsRequired(); // Configures the "TimeZoneId" property: maximum length 100, non-Unicode, and required
            builder.Property(x => x.Currency).HasMaxLength(3).IsUnicode(false).IsRequired(); // Configures the "Currency" property: maximum length 3, non-Unicode, and required
            builder.Property(x => x.Language).HasMaxLength(2).IsUnicode(false).IsRequired(); // Configures the "Language" property: maximum length 2, non-Unicode, and required

            builder.Property(x => x.TwoFactorEnabled).HasDefaultValue(false).IsRequired(); // Configures the "TwoFactorEnabled" property: default value false and required
            builder.Property(x => x.Status).IsRequired(); // Configures the "Status" property as required

            builder.Property(x => x.ConnectionStatus).IsRequired().HasDefaultValue(0); // Configures the "ConnectionStatus" property as required with a default value of 0 (Offline)

            builder.Property(x => x.LockedReason).HasMaxLength(250).IsRequired(); // Configures the "LockedReason" property: maximum length 250 and required

            // Configures the one-to-many relationship between ApplicationUser and Avatar
            builder.HasOne(x => x.Avartar) // ApplicationUser has one Avatar (nullable)
                .WithMany(x => x.Users) // Avatar can be associated with many ApplicationUsers
                .HasForeignKey(x => x.AvatarId) // Defines the foreign key "AvatarId"
                .OnDelete(DeleteBehavior.SetNull); // Configures the delete behavior: when an Avatar is deleted, the AvatarId of associated users is set to null

            // Configures the one-to-many relationship between ApplicationUser and ApplicationUserRole
            builder.HasMany(x => x.UserRoles) // ApplicationUser has many ApplicationUserRoles
                .WithOne(x => x.User) // Each ApplicationUserRole belongs to one ApplicationUser
                .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId" in the ApplicationUserRole table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserRoles are also deleted

            // Configures the one-to-many relationship between ApplicationUser and ApplicationUserClaim
            builder.HasMany(x => x.UserClaims) // ApplicationUser has many ApplicationUserClaims
                .WithOne(x => x.User) // Each ApplicationUserClaim belongs to one ApplicationUser
                .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId" in the ApplicationUserClaim table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserClaims are also deleted

            // Configures the one-to-many relationship between ApplicationUser and ApplicationUserLogin
            builder.HasMany(x => x.UserLogins) // ApplicationUser has many ApplicationUserLogins
                .WithOne(x => x.User) // Each ApplicationUserLogin belongs to one ApplicationUser
                .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId" in the ApplicationUserLogin table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserLogins are also deleted

            // Configures the one-to-many relationship between ApplicationUser and ApplicationUserToken
            builder.HasMany(x => x.UserTokens) // ApplicationUser has many ApplicationUserTokens
                .WithOne(x => x.User) // Each ApplicationUserToken belongs to one ApplicationUser
                .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId" in the ApplicationUserToken table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserTokens are also deleted

            // Configures the one-to-many relationship between ApplicationUser and ApplicationUserNotification
            builder.HasMany(x => x.UserNotifications) // ApplicationUser has many ApplicationUserNotifications
                .WithOne(x => x.User) // Each ApplicationUserNotification belongs to one ApplicationUser
                .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId" in the ApplicationUserNotification table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserNotifications are also deleted
        }
    }
}