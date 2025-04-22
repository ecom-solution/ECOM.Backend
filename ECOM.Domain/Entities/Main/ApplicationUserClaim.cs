namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a claim associated with an <see cref="ApplicationUser"/>.
    /// Claims are key-value pairs that represent specific rights or attributes of a user.
    /// This entity links a user to a specific claim definition.
    /// </summary>
    public class ApplicationUserClaim
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserClaim"/> class.
        /// </summary>
        public ApplicationUserClaim() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserClaim"/> class with specific user and claim identifiers.
        /// </summary>
        /// <param name="userId">The unique identifier of the <see cref="ApplicationUser"/>.</param>
        /// <param name="claimId">The unique identifier of the <see cref="ApplicationClaim"/>.</param>
        public ApplicationUserClaim(Guid userId, Guid claimId)
        {
            UserId = userId;
            ClaimId = claimId;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="ApplicationUser"/> to whom this claim belongs.
        /// This is a foreign key linking back to the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="ApplicationClaim"/> associated with the user.
        /// This is a foreign key linking to the claim definition.
        /// </summary>
        public Guid ClaimId { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ApplicationUser"/> entity.
        /// </summary>
        public virtual ApplicationUser? User { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ApplicationClaim"/> entity.
        /// </summary>
        public virtual ApplicationClaim? Claim { get; set; }
    }
}