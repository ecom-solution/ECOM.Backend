namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a claim associated with an <see cref="ApplicationRole"/>.
    /// Claims are key-value pairs that represent specific rights or attributes granted to a role.
    /// This entity links a role to a specific claim definition.
    /// </summary>
    public class ApplicationRoleClaim
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleClaim"/> class.
        /// </summary>
        public ApplicationRoleClaim() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleClaim"/> class with specific role and claim identifiers.
        /// </summary>
        /// <param name="roleId">The unique identifier of the <see cref="ApplicationRole"/>.</param>
        /// <param name="claimId">The unique identifier of the <see cref="ApplicationClaim"/>.</param>
        public ApplicationRoleClaim(Guid roleId, Guid claimId)
        {
            RoleId = roleId;
            ClaimId = claimId;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="ApplicationRole"/> to which this claim is granted.
        /// This is a foreign key linking back to the role.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="ApplicationClaim"/> associated with the role.
        /// This is a foreign key linking to the claim definition.
        /// </summary>
        public Guid ClaimId { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ApplicationRole"/> entity.
        /// </summary>
        public virtual ApplicationRole? Role { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ApplicationClaim"/> entity.
        /// </summary>
        public virtual ApplicationClaim? Claim { get; set; }
    }
}