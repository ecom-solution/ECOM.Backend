namespace ECOM.App.DTOs.Modules.Authentication.Users
{
	public class UserSignUp
	{
		public string UserName { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string TimeZoneId { get; set; } = string.Empty;
		public string Currency { get; set; } = string.Empty;
		public string Language { get; set; } = string.Empty;
	}
}
