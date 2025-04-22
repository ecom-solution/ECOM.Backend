namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents an external login (e.g., Facebook, Google) associated with an <see cref="ApplicationUser"/>.
    /// This entity allows users to log in to the application using credentials from external authentication providers.
    /// </summary>
    public class ApplicationUserLogin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserLogin"/> class.
        /// </summary>
        public ApplicationUserLogin() { }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="ApplicationUser"/> associated with this external login.
        /// This is a foreign key linking back to the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the external authentication provider (e.g., "Facebook", "Google", "Microsoft").
        /// Defaults to an empty string.
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier (key) assigned by the external provider to this user.
        /// This key is used to identify the user within the external system.
        /// Defaults to an empty string.
        /// </summary>
        public string ProviderKey { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the <see cref="ApplicationUser"/> entity associated with this external login.
        /// </summary>
        public virtual ApplicationUser? User { get; set; }
    }
}