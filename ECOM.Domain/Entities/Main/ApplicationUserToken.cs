namespace ECOM.Domain.Entities.Main
{
	public class ApplicationUserToken
    {
		public Guid UserId { get; set; }

		public string Provider { get; set; } = string.Empty;

		public string TokenName { get; set; } = string.Empty;

		public string TokenValue { get; set; } = string.Empty;

		public DateTime TokenExpiredAt_Utc { get; set; }

		public virtual ApplicationUser? User { get; set; }
	}
}
