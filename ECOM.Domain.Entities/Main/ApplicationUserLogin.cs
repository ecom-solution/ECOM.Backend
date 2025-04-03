namespace ECOM.Domain.Entities.Main
{
	public class ApplicationUserLogin
    {
		public ApplicationUserLogin() { }

		public Guid UserId { get; set; }

		public string Provider { get; set; } = string.Empty;

		public string ProviderKey { get; set; } = string.Empty;

		public virtual ApplicationUser? User { get; set; }
	}
}
