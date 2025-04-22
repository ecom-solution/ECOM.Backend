using ECOM.Shared.Library.Enums.Entity;

namespace ECOM.Domain.Entities.Main
{
	/// <summary>
	/// Represents a notification entity sent to one or more users.
	/// </summary>
	public class Notification : AuditableEntity
	{
		public Notification() { }

		/// <summary>
		/// The title or headline of the notification.
		/// </summary>
		public string Title { get; set; } = string.Empty;

		/// <summary>
		/// The content/body of the notification.
		/// </summary>
		public string Content { get; set; } = string.Empty;

		/// <summary>
		/// Type/category of the notification, stored as integer.
		/// </summary>
		public int Type { get; set; } = (int)NotificationTypeEnum.System;

		/// <summary>
		/// Optional links/actions associated with the notification.
		/// </summary>
		public virtual ICollection<NotificationLink>? NotificationLinks { get; set; }
	}
}
