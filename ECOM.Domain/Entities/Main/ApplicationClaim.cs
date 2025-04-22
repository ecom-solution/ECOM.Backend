namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a claim definition within the ECOM application.
    /// Claims are key-value pairs that represent specific rights or attributes
    /// that can be granted to users or roles.
    /// This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public class ApplicationClaim : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationClaim"/> class.
        /// </summary>
        public ApplicationClaim() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationClaim"/> class with specific claim type and value.
        /// </summary>
        /// <param name="claimType">The type or category of the claim (e.g., "permission", "role").</param>
        /// <param name="claimValue">The specific value of the claim (e.g., "product.create", "administrator").</param>
        public ApplicationClaim(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        /// <summary>
        /// Gets or sets the type or category of the claim (e.g., "permission", "role").
        /// This helps categorize and understand the purpose of the claim.
        /// Defaults to an empty string.
        /// </summary>
        public string ClaimType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the specific value associated with the claim type (e.g., "product.create", "administrator").
        /// This value defines the specific right or attribute.
        /// Defaults to an empty string.
        /// </summary>
        public string ClaimValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional description providing more context or information about this claim.
        /// Nullable.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="ApplicationRoleClaim"/> entities.
        /// These entities link roles to this claim definition.
        /// </summary>
        public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="ApplicationUserClaim"/> entities.
        /// These entities link users to this specific claim.
        /// </summary>
        public virtual ICollection<ApplicationUserClaim>? UserClaims { get; set; }
    }
}