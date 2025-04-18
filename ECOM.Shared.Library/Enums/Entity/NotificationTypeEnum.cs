namespace ECOM.Shared.Library.Enums.Entity
{
	/// <summary>
	/// Represents the type or category of a notification.
	/// Used to determine how the notification should be displayed or handled.
	/// </summary>
	public enum NotificationTypeEnum : int
	{
		/// <summary>
		/// A system-level notification, typically related to platform updates or technical issues.
		/// </summary>
		System = 0,

		/// <summary>
		/// A notification related to an order, such as order confirmation or shipping updates.
		/// </summary>
		Order = 1,

		/// <summary>
		/// A marketing or promotional notification (e.g., discount, offer, campaign).
		/// </summary>
		Promotion = 2,

		/// <summary>
		/// A scheduled or triggered reminder, such as a payment or subscription reminder.
		/// </summary>
		Reminder = 3,

		/// <summary>
		/// A custom or generic notification that does not fall into predefined categories.
		/// </summary>
		Custom = 4
	}
}
