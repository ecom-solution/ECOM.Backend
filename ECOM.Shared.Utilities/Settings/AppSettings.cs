namespace ECOM.Shared.Utilities.Settings
{
	public class AppSettings
	{
		public AuthenticationSetting Authentication { get; set; } = new AuthenticationSetting();
		public DbContextSetting DbContext { get; set; } = new DbContextSetting();
		public SmtpSetting Smtp { get; set; } = new SmtpSetting();
		public StorageSetting Storage { get; set; } = new StorageSetting();
		public LoggingSetting Logging { get; set; } = new LoggingSetting();

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
			public string User { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
			public string Host { get; set; } = string.Empty;
			public int Port { get; set; } = 0;
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

		}
	}
}
