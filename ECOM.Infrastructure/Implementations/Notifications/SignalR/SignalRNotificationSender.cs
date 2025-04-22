using ECOM.Domain.Interfaces.Notifications;
using ECOM.Shared.Library.Models.Externals.RabbitMQ;
using Microsoft.AspNetCore.SignalR;

namespace ECOM.Infrastructure.Implementations.Notifications.SignalR
{
	public class SignalRNotificationSender(IHubContext<NotificationHub> hubContext) : INotificationSender
	{
		private readonly IHubContext<NotificationHub> _hubContext = hubContext;

		public async Task SendAsync(NotificationMessage message)
		{
			if (string.IsNullOrEmpty(message.TargetUserId))
			{
				await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
			}
			else
			{
				await _hubContext.Clients.User(message.TargetUserId)
					.SendAsync("ReceiveNotification", message);
			}
		}
	}
}
