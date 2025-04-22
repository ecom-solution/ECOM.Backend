using ECOM.Domain.Interfaces.Emails;
using ECOM.Shared.Library.Models.Externals.RabbitMQ;
using MassTransit;

namespace ECOM.Infrastructure.Implementations.Messagings.Consumers
{
	public class EmailConsumer(IEmailSender sender) : IConsumer<EmailMessage>
	{
		private readonly IEmailSender _emailSender = sender;

		public async Task Consume(ConsumeContext<EmailMessage> context)
		{
			await _emailSender.SendAsync(context.Message);
		}
	}
}
