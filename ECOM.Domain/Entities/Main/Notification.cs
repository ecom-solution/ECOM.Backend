using ECOM.Shared.Library.Enums.Entity;

namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a notification entity within the ECOM system that can be sent to one or more users.
    /// This class inherits from <see cref="AuditableEntity"/>, providing common auditing properties.
    /// </summary>
    public class Notification : AuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        public Notification() { }

        /// <summary>
        /// Gets or sets the main title or headline of the notification.
        /// Defaults to an empty string.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the detailed content or body of the notification message.
        /// Defaults to an empty string.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type or category of the notification, represented by an integer
        /// corresponding to the <see cref="NotificationTypeEnum"/> enumeration.
        /// Defaults to <c>(int)NotificationTypeEnum.System</c>.
        /// </summary>
        public int Type { get; set; } = (int)NotificationTypeEnum.System;

        /// <summary>
        /// Navigation property representing the optional links or actions associated with this notification.
        /// These links allow users to interact with the notification content.
        /// </summary>
        public virtual ICollection<NotificationLink>? NotificationLinks { get; set; }

        /// <summary>
        /// Navigation property representing the status of this notification for different users
        /// (e.g., whether a user has seen it). This is managed through the <see cref="ApplicationUserNotification"/> entity.
        /// </summary>
        public virtual ICollection<ApplicationUserNotification>? UserNotifications { get; set; }
    }
}