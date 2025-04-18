using ECOM.Domain.Interfaces.Messagings;
using MassTransit;

namespace ECOM.Infrastructure.Implementations.Messagings
{
	public class RabbitMQPublisher(IPublishEndpoint publishEndpoint) : IPublisher
	{
		private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

		public async Task PublishAsync<T>(T message, string routingKey) where T : notnull
		{
			await _publishEndpoint.Publish(message);
		}

	}
}
