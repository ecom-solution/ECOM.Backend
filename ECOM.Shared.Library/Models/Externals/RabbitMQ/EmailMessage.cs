namespace ECOM.Shared.Library.Models.Externals.RabbitMQ
{
	public class EmailMessage
	{
		/// <summary>
		/// The recipient's email address.
		/// </summary>
		public string To { get; set; } = string.Empty;

		/// <summary>
		/// The subject of the email.
		/// </summary>
		public string Subject { get; set; } = string.Empty;

		/// <summary>
		/// The body content of the email.
		/// </summary>
		public string Body { get; set; } = string.Empty;

		/// <summary>
		/// Optional list of attachments. Each item contains file name and content in Base64 format.
		/// </summary>
		public List<EmailAttachment> Attachments { get; set; } = [];
	}

	public class EmailAttachment
	{
		/// <summary>
		/// The display name of the attachment (e.g., "invoice.pdf").
		/// </summary>
		public string FileName { get; set; } = string.Empty;

		/// <summary>
		/// The URL or storage path where the file is stored.
		/// </summary>
		public string FileUrl { get; set; } = string.Empty;

		/// <summary>
		/// (Optional) MIME type of the file (e.g., "application/pdf").
		/// </summary>
		public string ContentType { get; set; } = string.Empty;
	}
}
