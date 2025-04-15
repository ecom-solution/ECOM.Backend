namespace ECOM.Shared.Library.Models.Dtos.Modules.Authentication.Users
{
	public class UserSignIn
	{
		public string UserName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}

	public class UserSignedIn
	{
		public Guid Id { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string TimeZoneId { get; set; } = string.Empty;
		public string Currency { get; set; } = string.Empty;
		public string Language { get; set; } = string.Empty;
		public string SecretKey { get; set; } = string.Empty;
		public string AccessToken { get; set; } = string.Empty;
		public bool TwoFactorEnabled { get; set; } = false;
		public string QRCodeUri { get; set; } = string.Empty;
	}
}
