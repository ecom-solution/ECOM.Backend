using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationUserNotification"/> entity, defining its mapping to the database table,
    /// composite primary key, properties related to the user's notification status, and its relationships with
    /// <see cref="ApplicationUser"/> and <see cref="Notification"/> entities.
    /// This class implements the <see cref="IEntityTypeConfiguration{ApplicationUserNotification}"/> interface.
    /// </summary>
    public class ApplicationUserNotificationConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUserNotification>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationUserNotification"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationUserNotification> builder)
        {
            builder.ToTable(nameof(ApplicationUserNotification)); // Maps the entity to a table named "ApplicationUserNotification"

            // Configures the composite primary key consisting of UserId and NotificationId
            builder.HasKey(x => new { x.UserId, x.NotificationId });

            builder.Property(x => x.IsSeen)
                .IsRequired()
                .HasDefaultValue(false); // Configures the "IsSeen" property as required with a default value of false (unread)

            builder.Property(x => x.SeenDate_Utc); // Configures the "SeenDate_Utc" property to store the UTC date when the notification was seen

            // Configures the many-to-one relationship with the ApplicationUser entity
            builder.HasOne(x => x.User) // ApplicationUserNotification belongs to one ApplicationUser
                .WithMany(x => x.UserNotifications) // ApplicationUser can have many ApplicationUserNotifications
                .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId"
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserNotifications are also deleted

            // Configures the many-to-one relationship with the Notification entity
            builder.HasOne(x => x.Notification) // ApplicationUserNotification belongs to one Notification
                .WithMany(x => x.UserNotifications) // Notification can have many ApplicationUserNotifications
                .HasForeignKey(x => x.NotificationId) // Defines the foreign key "NotificationId"
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a Notification is deleted, all associated ApplicationUserNotifications are also deleted
        }
    }
}