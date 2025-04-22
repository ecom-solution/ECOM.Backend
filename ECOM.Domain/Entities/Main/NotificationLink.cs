namespace ECOM.Domain.Entities.Main
{
	/// <summary>
	/// Represents a hyperlink or action button associated with a notification.
	/// Allows a notification to contain multiple links for user interaction.
	/// </summary>
	public class NotificationLink : AuditableEntity
	{
		/// <summary>
		/// The ID of the parent notification this link belongs to.
		/// </summary>
		public Guid NotificationId { get; set; }

		/// <summary>
		/// The display label for the link (e.g., "View Order", "Track Shipment").
		/// </summary>
		public string Label { get; set; } = string.Empty;

		/// <summary>
		/// The URL or action target that the link points to.
		/// Can be a relative app route or an external URL.
		/// </summary>
		public string Url { get; set; } = string.Empty;

		/// <summary>
		/// Determines the display order of the link relative to others (ascending).
		/// </summary>
		public int OrderIndex { get; set; } = 0;

		/// <summary>
		/// Navigation property to the parent notification.
		/// </summary>
		public virtual Notification? Notification { get; set; }
	}

}
