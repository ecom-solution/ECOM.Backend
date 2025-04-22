namespace ECOM.Domain.Entities.Main
{
	public class ApplicationClaim : BaseEntity
    {
        public ApplicationClaim() { }
		public ApplicationClaim(string claimType, string claimValue) 
		{
			ClaimType = claimType;
			ClaimValue = claimValue;
		}

		public string ClaimType { get; set; } = string.Empty;
        public string ClaimValue { get; set;} = string.Empty;
        public string? Description { get; set; }

		public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }
		public virtual ICollection<ApplicationUserClaim>? UserClaims { get; set; }
	}
}
