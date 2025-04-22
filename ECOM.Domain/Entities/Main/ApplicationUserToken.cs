namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents an authentication token associated with an <see cref="ApplicationUser"/>.
    /// These tokens can be used for various purposes, such as password reset, email confirmation,
    /// or other security-related flows.
    /// </summary>
    public class ApplicationUserToken
    {
        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="ApplicationUser"/> to whom this token belongs.
        /// This is a foreign key linking back to the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the provider that issued this token (e.g., "Local", "Facebook", "Google").
        /// Defaults to an empty string.
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name or type of the token (e.g., "PasswordReset", "EmailConfirmation", "RefreshToken").
        /// Defaults to an empty string.
        /// </summary>
        public string TokenName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the actual value of the token. This should be a securely generated string.
        /// Defaults to an empty string.
        /// </summary>
        public string TokenValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the UTC date and time when this token expires and is no longer valid.
        /// </summary>
        public DateTime TokenExpiredAt_Utc { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ApplicationUser"/> entity associated with this token.
        /// </summary>
        public virtual ApplicationUser? User { get; set; }
    }
}