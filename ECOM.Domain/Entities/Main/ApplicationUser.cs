using ECOM.Shared.Library.Enums.Entity;

namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents an application user within the ECOM system, encompassing authentication details,
    /// profile information, and various system-related preferences and statuses.
    /// This class inherits from <see cref="AuditableEntity"/>, providing common auditing properties.
    /// </summary>
    public class ApplicationUser : AuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUser"/> class.
        /// </summary>
        public ApplicationUser() { }

        /// <summary>
        /// Gets or sets the unique username for the user. This is used for login purposes.
        /// Defaults to an empty string.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the normalized version of the username, typically converted to uppercase
        /// for case-insensitive comparisons during authentication.
        /// Defaults to an empty string.
        /// </summary>
        public string NormalizedUserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's full name as displayed in the application.
        /// Defaults to an empty string.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the normalized version of the full name, used for case-insensitive searching and sorting.
        /// Defaults to an empty string.
        /// </summary>
        public string NormalizedFullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's email address, used for communication and account recovery.
        /// Defaults to an empty string.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the normalized version of the email address, used for case-insensitive comparisons.
        /// Defaults to an empty string.
        /// </summary>
        public string NormalizedEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a boolean value indicating whether the user's email address has been verified.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool EmailConfirmed { get; set; } = false;

        /// <summary>
        /// Gets or sets the user's primary phone number.
        /// Defaults to an empty string.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a boolean value indicating whether the user's phone number has been verified.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; } = false;

        /// <summary>
        /// Gets or sets the securely hashed representation of the user's password.
        /// Defaults to an empty string.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a secret key used for various security features, such as generating tokens or encryption.
        /// Defaults to an empty string.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a security stamp used to invalidate security tokens if user details change.
        /// Defaults to an empty string.
        /// </summary>
        public string SecurityStamp { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a concurrency stamp used for optimistic locking during updates to prevent data collisions.
        /// Defaults to an empty string.
        /// </summary>
        public string ConcurrencyStamp { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's preferred time zone identifier, following IANA or Windows standards.
        /// Defaults to an empty string.
        /// </summary>
        public string TimeZoneId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's preferred currency code (e.g., "USD", "VND").
        /// Defaults to an empty string.
        /// </summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's preferred language code (e.g., "en-US", "vi-VN").
        /// Defaults to an empty string.
        /// </summary>
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a boolean value indicating whether two-factor authentication is enabled for the user.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool TwoFactorEnabled { get; set; } = false;

        /// <summary>
        /// Gets or sets the number of consecutive failed login attempts for the user.
        /// Nullable.
        /// </summary>
        public int? AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the number of failed attempts for verification processes (e.g., OTP verification).
        /// Nullable.
        /// </summary>
        public int? VerifyFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the current status of the user account, represented by the <see cref="UserStatusEnum"/> integer value.
        /// Defaults to <c>(int)UserStatusEnum.New</c>.
        /// </summary>
        public int Status { get; set; } = (int)UserStatusEnum.New;

        /// <summary>
        /// Gets or sets the current real-time connection status of the user, represented by the <see cref="ConnectionStatusEnum"/> integer value.
        /// Defaults to <c>(int)ConnectionStatusEnum.Offline</c>.
        /// </summary>
        public int ConnectionStatus { get; set; } = (int)ConnectionStatusEnum.Offline;

        /// <summary>
        /// Gets or sets the reason why the user account is currently locked out, if applicable.
        /// Defaults to an empty string.
        /// </summary>
        public string LockedReason { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user's date of birth. Nullable.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the last date and time (in UTC) when the user was detected as online. Nullable.
        /// </summary>
        public DateTime? LastSeenAt_Utc { get; set; }

        /// <summary>
        /// Gets or sets the date and time (in UTC) until which the user account is locked out, if applicable. Nullable.
        /// </summary>
        public DateTime? LockoutEndDate_Utc { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user's avatar file, referencing the <see cref="FileEntity"/>. Nullable.
        /// </summary>
        public Guid? AvatarId { get; set; }

        /// <summary>
        /// Navigation property to the user's avatar file entity. Nullable.
        /// </summary>
        public virtual FileEntity? Avartar { get; set; }

        /// <summary>
        /// Navigation property representing the roles assigned to this user.
        /// </summary>
        public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }

        /// <summary>
        /// Navigation property representing the claims associated with this user.
        /// </summary>
        public virtual ICollection<ApplicationUserClaim>? UserClaims { get; set; }

        /// <summary>
        /// Navigation property representing the external login providers linked to this user.
        /// </summary>
        public virtual ICollection<ApplicationUserLogin>? UserLogins { get; set; }

        /// <summary>
        /// Navigation property representing the authentication tokens associated with this user.
        /// </summary>
        public virtual ICollection<ApplicationUserToken>? UserTokens { get; set; }

        /// <summary>
        /// Navigation property representing the notifications that have been sent to this user
        /// and their read/unread status.
        /// </summary>
        public virtual ICollection<ApplicationUserNotification>? UserNotifications { get; set; }
    }
}