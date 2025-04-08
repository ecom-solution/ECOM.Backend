namespace ECOM.App.DTOs.Modules.Authentication.Users
{
	public class UserSignOut
	{
		public Guid Id { get; set; }
		public string? RefreshToken { get; set; }
	}

	public class UserSignedOut
	{
		public Guid Id { get; set; }
		public DateTime SignedOutAt_Utc { get; set; } = DateTime.UtcNow;
	}
}
