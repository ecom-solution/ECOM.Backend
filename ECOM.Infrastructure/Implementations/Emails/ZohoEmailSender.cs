using ECOM.Domain.Interfaces.Emails;
using ECOM.Shared.Library.Models.Externals.RabbitMQ;
using ECOM.Shared.Library.Models.Settings;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace ECOM.Infrastructure.Implementations.Emails
{
	public class ZohoEmailSender(IOptions<AppSettings> appSettings) : IEmailSender
	{
		private readonly AppSettings _appSettings = appSettings.Value;

		public async Task SendAsync(EmailMessage message)
		{
			using var smtpClient = new SmtpClient(_appSettings.Smtp.Host, _appSettings.Smtp.Port)
			{
				Credentials = new NetworkCredential(_appSettings.Smtp.User, _appSettings.Smtp.Password),
				EnableSsl = _appSettings.Smtp.EnableSsl
			};

			var mail = new MailMessage
			{
				From = new MailAddress(_appSettings.Smtp.User),
				Subject = message.Subject,
				Body = message.Body,
				IsBodyHtml = true
			};

			mail.To.Add(message.To);

			// Optionally add attachments
			if (message.Attachments != null)
			{
				foreach (var att in message.Attachments)
				{
					// Assuming attachments use FileUrl (public or local file path)
					mail.Attachments.Add(new Attachment(att.FileUrl)
					{
						Name = att.FileName,
						ContentType = new System.Net.Mime.ContentType(att.ContentType)
					});
				}
			}

			await smtpClient.SendMailAsync(mail);
		}
	}
}
