namespace ECOM.Shared.Library.Models.Externals.RabbitMQ
{
	public class NotificationMessage
	{
		/// <summary>
		/// The ID of the target user who will receive the notification.
		/// Leave null if it's a broadcast.
		/// </summary>
		public string? TargetUserId { get; set; }

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
		public string Type { get; set; } = string.Empty;

		/// <summary>
		/// Optional URL or deep link the user can open when interacting with the notification.
		/// </summary>
		public string? Link { get; set; }

		/// <summary>
		/// The time when the notification was created.
		/// </summary>
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
