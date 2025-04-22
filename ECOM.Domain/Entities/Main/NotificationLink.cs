namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a hyperlink or action button that is associated with a <see cref="Notification"/> entity.
    /// This allows notifications to include interactive elements that users can click or tap.
    /// This class inherits from <see cref="AuditableEntity"/>, providing common auditing properties.
    /// </summary>
    public class NotificationLink : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the parent <see cref="Notification"/> to which this link belongs.
        /// This establishes the relationship between the link and the notification.
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the text that is displayed to the user for this link or button
        /// (e.g., "View Details", "Confirm Action"). Defaults to an empty string.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the target URL or application route that this link navigates to when clicked.
        /// This can be an internal path within the application or an external web address.
        /// Defaults to an empty string.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an integer value that determines the order in which this link is displayed
        /// relative to other links within the same notification. Links with lower `OrderIndex` values
        /// are typically displayed before those with higher values. Defaults to 0.
        /// </summary>
        public int OrderIndex { get; set; } = 0;

        /// <summary>
        /// Navigation property to the parent <see cref="Notification"/> entity.
        /// This allows easy access to the notification that contains this link.
        /// </summary>
        public virtual Notification? Notification { get; set; }
    }
}