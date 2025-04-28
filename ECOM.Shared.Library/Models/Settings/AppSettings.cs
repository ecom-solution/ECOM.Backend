namespace ECOM.Shared.Library.Models.Settings
{
	public class AppSettings
	{
		public AuthenticationSetting Authentication { get; set; } = new AuthenticationSetting();
		public DbContextSetting DbContext { get; set; } = new DbContextSetting();
		public SmtpSetting Smtp { get; set; } = new SmtpSetting();
		public StorageSetting Storage { get; set; } = new StorageSetting();
		public LoggingSetting Logging { get; set; } = new LoggingSetting();
		public RabbitMQSetting RabbitMQ { get; set; } = new RabbitMQSetting();
		public SignalIRSetting SignalIR { get; set; } = new SignalIRSetting();

        public class AuthenticationSetting
		{
			public int MaxVerifyFailedCount { get; set; } = 0;
			public int MaxAccessFailedCount { get; set; } = 0;
			public int NumberOfDaysLocked { get; set; } = 0;
			public JwtSetting Jwt { get; set; } = new JwtSetting();
			public CorsSetting Cors { get; set; } = new CorsSetting();

			public class JwtSetting
			{
				public string SecretKey { get; set; } = string.Empty;
				public string ValidIssuer { get; set; } = string.Empty;
				public string ValidAudience { get; set; } = string.Empty;
				public int AccessTokenValidityInMinutes { get; set; } = 0;
				public int RefreshTokenValidityInDays { get; set; } = 0;
			}

			public class CorsSetting
			{
				public string[] AllowedOrigins { get; set; } = [];
			}
		}

		public class DbContextSetting
		{
			public RetrySetting Retry { get; set; } = new RetrySetting();
			public BulkSetting Bulk { get; set; } = new BulkSetting();

			public class RetrySetting
			{
				public int MaxAttemptCount { get; set; } = 0;
				public int IntervalInSeconds { get; set; } = 0;
			}

			public class BulkSetting
			{
				public int BatchSize { get; set; } = 0;
				public int CmdTimeOutInMiliseconds { get; set; } = 0;
			}
		}

		public class SmtpSetting
		{
			/// <summary>
			/// The SMTP username (usually your email address).
			/// </summary>
			public string User { get; set; } = string.Empty;

			/// <summary>
			/// The SMTP password or app password if 2FA is enabled.
			/// </summary>
			public string Password { get; set; } = string.Empty;

			/// <summary>
			/// The SMTP host (e.g., smtp.zoho.com, smtp.gmail.com).
			/// </summary>
			public string Host { get; set; } = string.Empty;

			/// <summary>
			/// The port to connect to (e.g., 465 for SSL, 587 for TLS).
			/// </summary>
			public int Port { get; set; } = 0;

			/// <summary>
			/// Indicates whether to use SSL for the connection.
			/// </summary>
			public bool EnableSsl { get; set; } = true;

			/// <summary>
			/// Indicates whether 2FA is enabled. If true, an App Password should be used.
			/// </summary>
			public bool UseAppPassword { get; set; } = false;
		}

		public class StorageSetting
		{
			public string Endpoint { get; set; } = string.Empty;
			public int Port { get; set; } = 0;
			public string AccessKey { get; set; } = string.Empty;
			public string SecretKey { get; set; } = string.Empty;
			public bool UseSSL { get; set; } = false;
		}

		public class LoggingSetting
		{
			public string MinimumLevel { get; set; } = string.Empty;
			public string MaximumLevel { get; set; } = string.Empty;
		}

		public class RabbitMQSetting
		{
			/// <summary>
			/// The hostname or IP address of the RabbitMQ server.
			/// Example: "localhost", "rabbitmq.example.com"
			/// </summary>
			public string Host { get; set; } = string.Empty;

			/// <summary>
			/// The RabbitMQ virtual host.
			/// Default is "/" (root). Can be custom, such as "/dev" or "/production".
			/// </summary>
			public string VirtualHost { get; set; } = string.Empty;

			/// <summary>
			/// The TCP port used to connect to RabbitMQ.
			/// Default: 5672 (non-SSL), 5671 (SSL)
			/// </summary>
			public int Port { get; set; }

			/// <summary>
			/// Whether to use SSL (TLS) for the connection.
			/// </summary>
			public bool UseSsl { get; set; }

			/// <summary>
			/// Username to authenticate with RabbitMQ.
			/// Default for local: "guest"
			/// </summary>
			public string Username { get; set; } = string.Empty;

			/// <summary>
			/// Password to authenticate with RabbitMQ.
			/// </summary>
			public string Password { get; set; } = string.Empty;

			/// <summary>
			/// Name of the main queue for email-related messages.
			/// Example: "email-queue"
			/// </summary>
			public string EmailQueue { get; set; } = string.Empty;

			/// <summary>
			/// Name of the dead-letter queue (DLQ) for email queue.
			/// Stores messages that fail processing (e.g. exceptions, retries exceeded).
			/// Example: "email-queue-dlq"
			/// </summary>
			public string EmailQueueDLQ { get; set; } = string.Empty;

			/// <summary>
			/// Name of the main queue for notification-related messages.
			/// Example: "notify-queue"
			/// </summary>
			public string NotifyQueue { get; set; } = string.Empty;

			/// <summary>
			/// Name of the dead-letter queue (DLQ) for notification queue.
			/// Example: "notify-queue-dlq"
			/// </summary>
			public string NotifyQueueDLQ { get; set; } = string.Empty;

			public string LocalizationContentGenerateQueue { get; set; } = string.Empty;

            public string LocalizationContentGenerateQueueDLQ { get; set; } = string.Empty;

        }
	
		public class SignalIRSetting
		{
			public string ReceiveMethod { get; set; } = string.Empty;

        }
	}
}
