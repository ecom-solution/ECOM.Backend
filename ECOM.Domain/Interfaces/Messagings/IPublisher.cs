namespace ECOM.Domain.Interfaces.Messagings
{
	public interface IPublisher
	{
		/// <summary>
		/// Publishes a message to the specified queue or exchange asynchronously.
		/// </summary>
		/// <typeparam name="T">The type of the message.</typeparam>
		/// <param name="message">The message object to publish.</param>
		/// <param name="routingKey">The routing key or queue name (optional in MassTransit).</param>
		Task PublishAsync<T>(T message, string routingKey) where T : notnull;
	}
}
