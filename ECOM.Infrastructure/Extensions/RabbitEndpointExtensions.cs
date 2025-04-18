using MassTransit;

namespace ECOM.Infrastructure.Extensions
{
	public static class RabbitEndpointExtensions
	{
		public static void BindDeadLetter(this IRabbitMqReceiveEndpointConfigurator endpoint, string deadLetterQueueName)
		{
			endpoint.SetQueueArgument("x-dead-letter-exchange", "");
			endpoint.SetQueueArgument("x-dead-letter-routing-key", deadLetterQueueName);
		}
	}
}
