using ECOM.Shared.Library.Enums.Entity;

namespace ECOM.Shared.Library.Models.Externals.RabbitMQ
{
    public class NotificationMessage
    {
        /// <summary>
        /// The IDs of the target users who will receive the notification.
        /// Leave null or empty if it's a broadcast.
        /// </summary>
        public Guid[] TargetUserIds { get; set; } = [];

        /// <summary>
        /// The title of the notification.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The main body content of the notification.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// The type or category of the notification (e.g., "System", "Order", "Promotion").
        /// </summary>
        public int Type { get; set; } = (int)NotificationTypeEnum.System;

        /// <summary>
        /// Optional links or actions associated with this notification.
        /// </summary>
        public List<NotificationLinkMessage>? Links { get; set; }

        /// <summary>
        /// The time when the notification was created.
        /// </summary>
        public DateTime CreatedAt_Utc { get; set; } = DateTime.UtcNow;
    }

    public class NotificationLinkMessage
    {
        /// <summary>
        /// The text that is displayed to the user for this link or button.
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// The target URL or application route that this link navigates to when clicked.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// An integer value that determines the order in which this link is displayed.
        /// </summary>
        public int OrderIndex { get; set; } = 0;
    }
}