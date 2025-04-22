using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="Notification"/> entity, defining its mapping to the database table,
    /// properties related to notifications, and its relationship with <see cref="NotificationLink"/> and <see cref="ApplicationUserNotification"/>.
    /// This class implements the <see cref="IEntityTypeConfiguration{Notification}"/> interface.
    /// </summary>
    public class NotificationConfiguration : MainConfiguration, IEntityTypeConfiguration<Notification>
    {
        /// <summary>
        /// Configures the <see cref="Notification"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(nameof(Notification)); // Maps the entity to a table named "Notification"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key

            builder.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired(); // Configures the "Title" property: maximum length 200 and required

            builder.Property(x => x.Content)
                .IsRequired(); // Configures the "Content" property as required

            builder.Property(x => x.Type)
                .IsRequired(); // Configures the "Type" property as required

            // Configures the one-to-many relationship between Notification and NotificationLink
            builder.HasMany(x => x.NotificationLinks) // Notification has many NotificationLinks
                .WithOne(x => x.Notification) // Each NotificationLink belongs to one Notification
                .HasForeignKey(x => x.NotificationId) // Defines the foreign key "NotificationId" in the NotificationLink table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a Notification is deleted, all associated NotificationLinks are also deleted

            // Configures the one-to-many relationship between Notification and ApplicationUserNotification
            builder.HasMany(x => x.UserNotifications) // Notification has many ApplicationUserNotifications
                .WithOne(x => x.Notification) // Each ApplicationUserNotification belongs to one Notification
                .HasForeignKey(x => x.NotificationId) // Defines the foreign key "NotificationId" in the ApplicationUserNotification table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a Notification is deleted, all associated ApplicationUserNotifications are also deleted
        }
    }
}