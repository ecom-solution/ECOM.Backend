namespace ECOM.Domain.Entities.Main
{
	public class ApplicationRoleClaim
    {
        public ApplicationRoleClaim() { }
		public ApplicationRoleClaim(Guid roleId, Guid claimId) 
        {
            RoleId = roleId;
            ClaimId = claimId;
        }
		public Guid RoleId { get; set; }
        public Guid ClaimId { get; set; }
        public virtual ApplicationRole? Role { get; set; }
		public virtual ApplicationClaim? Claim { get; set; }
    }
}
