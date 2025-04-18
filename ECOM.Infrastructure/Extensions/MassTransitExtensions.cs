using MassTransit;

namespace ECOM.Infrastructure.Extensions
{
	public static class MassTransitExtensions
	{
		/// <summary>
		/// Binds a custom dead-letter queue (DLQ) to the RabbitMQ endpoint.
		/// </summary>
		/// <param name="endpoint">The RabbitMQ receive endpoint configurator.</param>
		/// <param name="deadLetterQueueName">The name of the dead-letter queue to bind.</param>
		public static void BindDeadLetterQueue(this IReceiveEndpointConfigurator endpoint, string deadLetterQueueName)
		{
			if (string.IsNullOrWhiteSpace(deadLetterQueueName)) return;

			if (endpoint is IRabbitMqReceiveEndpointConfigurator rabbitCfg)
			{
				rabbitCfg.ConfigureConsumeTopology = false;

				rabbitCfg.Bind(deadLetterQueueName, s =>
				{
					s.RoutingKey = "#";
					s.ExchangeType = "fanout";
				});
			}
			else
			{
				throw new InvalidOperationException("Dead-letter queue binding is only supported with RabbitMQ.");
			}
		}
	}
}
