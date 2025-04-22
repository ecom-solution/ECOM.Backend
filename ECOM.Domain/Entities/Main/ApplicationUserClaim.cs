namespace ECOM.Domain.Entities.Main
{
	public class ApplicationUserClaim
    {
		public ApplicationUserClaim() { }
		
		public ApplicationUserClaim(Guid userId, Guid claimId)
		{
			UserId = userId;
			ClaimId = claimId;
		}

		public Guid UserId { get; set; }

		public Guid ClaimId { get; set; }

		public virtual ApplicationUser? User { get; set; }
		public virtual ApplicationClaim? Claim { get; set; }
	}
}
