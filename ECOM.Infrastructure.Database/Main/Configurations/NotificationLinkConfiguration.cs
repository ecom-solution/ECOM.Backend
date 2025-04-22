using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="NotificationLink"/> entity, defining its mapping to the database table,
    /// properties related to links within a notification, and its relationship with the <see cref="Notification"/> entity.
    /// This class implements the <see cref="IEntityTypeConfiguration{NotificationLink}"/> interface.
    /// </summary>
    public class NotificationLinkConfiguration : MainConfiguration, IEntityTypeConfiguration<NotificationLink>
    {
        /// <summary>
        /// Configures the <see cref="NotificationLink"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<NotificationLink> builder)
        {
            builder.ToTable(nameof(NotificationLink)); // Maps the entity to a table named "NotificationLink"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key

            builder.Property(x => x.Label)
                .HasMaxLength(100)
                .IsRequired(); // Configures the "Label" property: maximum length 100 and required (text displayed for the link)

            builder.Property(x => x.Url)
                .HasMaxLength(1000)
                .IsRequired(); // Configures the "Url" property: maximum length 1000 and required (the target URL of the link)

            builder.Property(x => x.OrderIndex)
                .IsRequired(); // Configures the "OrderIndex" property as required (determines the order of links in a notification)

            builder.Property(x => x.NotificationId)
                .IsRequired(); // Configures the "NotificationId" property as required (foreign key to the Notification entity)

            // Configures the many-to-one relationship between NotificationLink and Notification
            builder.HasOne(x => x.Notification) // NotificationLink belongs to one Notification
                .WithMany(x => x.NotificationLinks) // Notification has many NotificationLinks
                .HasForeignKey(x => x.NotificationId) // Defines the foreign key "NotificationId"
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a Notification is deleted, all associated NotificationLinks are also deleted
        }
    }
}