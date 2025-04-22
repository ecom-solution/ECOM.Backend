using ECOM.Shared.Library.Models.Externals.RabbitMQ;

namespace ECOM.Domain.Interfaces.Emails
{
	public interface IEmailSender
	{
		/// <summary>
		/// Sends an email based on the provided message object.
		/// </summary>
		/// <param name="message">The email message details including recipient, subject, body, and attachments.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		Task SendAsync(EmailMessage message);
	}
}
