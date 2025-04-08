namespace ECOM.App.DTOs.Modules.Authentication.Users
{
	public class UserToken
	{
		public Guid UserId { get; set; }

		public string Provider { get; set; } = string.Empty;

		public string TokenName { get; set; } = string.Empty;

		public string TokenValue { get; set; } = string.Empty;

		public DateTime TokenExpiredAt_Utc { get; set; }
	}
}
