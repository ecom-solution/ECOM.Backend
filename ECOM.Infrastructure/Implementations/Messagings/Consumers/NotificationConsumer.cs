using ECOM.Domain.Interfaces.Notifications;
using ECOM.Shared.Library.Models.Externals.RabbitMQ;
using MassTransit;

namespace ECOM.Infrastructure.Implementations.Messagings.Consumers
{
	public class NotificationConsumer(INotificationSender notificationSender) : IConsumer<NotificationMessage>
	{
		private readonly INotificationSender _notificationSender = notificationSender;

		public async Task Consume(ConsumeContext<NotificationMessage> context)
		{
			await _notificationSender.SendAsync(context.Message);
		}
	}
}
