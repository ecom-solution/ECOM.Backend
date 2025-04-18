using ECOM.Shared.Library.Models.Externals.RabbitMQ;

namespace ECOM.Domain.Interfaces.Notifications
{
	public interface INotificationSender
	{
		/// <summary>
		/// Sends a notification asynchronously.
		/// </summary>
		/// <param name="message">The notification message to send.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task SendAsync(NotificationMessage message);
	}
}
