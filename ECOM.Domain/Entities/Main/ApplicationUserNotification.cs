namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents the status of a notification for a specific application user,
    /// indicating whether the user has seen the notification.
    /// </summary>
    public class ApplicationUserNotification
    {
        public ApplicationUserNotification() { }

        /// <summary>
        /// Gets or sets the unique identifier of the application user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the notification.
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether the user has seen the notification.
        /// Defaults to false.
        /// </summary>
        public bool IsSeen { get; set; } = false;

        /// <summary>
        /// Gets or sets the date and time when the user marked the notification as seen (UTC).
        /// Can be null if the notification hasn't been seen.
        /// </summary>
        public DateTime? SeenDate_Utc { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ApplicationUser"/> entity.
        /// </summary>
        public virtual ApplicationUser? User { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="Notification"/> entity.
        /// </summary>
        public virtual Notification? Notification { get; set; }
    }
}
