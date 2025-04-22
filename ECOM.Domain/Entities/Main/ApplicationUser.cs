using ECOM.Shared.Library.Enums.Entity;

namespace ECOM.Domain.Entities.Main
{
	/// <summary>
	/// Represents an application user with authentication, profile, and system-related properties.
	/// </summary>
	public class ApplicationUser : AuditableEntity
	{
		public ApplicationUser() { }

		/// <summary>
		/// The unique username of the user.
		/// </summary>
		public string UserName { get; set; } = string.Empty;

		/// <summary>
		/// Normalized version of the username (usually uppercase).
		/// </summary>
		public string NormalizedUserName { get; set; } = string.Empty;

		/// <summary>
		/// The full name of the user.
		/// </summary>
		public string FullName { get; set; } = string.Empty;

		/// <summary>
		/// Normalized version of the full name.
		/// </summary>
		public string NormalizedFullName { get; set; } = string.Empty;

		/// <summary>
		/// The email address of the user.
		/// </summary>
		public string Email { get; set; } = string.Empty;

		/// <summary>
		/// Normalized version of the email address.
		/// </summary>
		public string NormalizedEmail { get; set; } = string.Empty;

		/// <summary>
		/// Indicates whether the user's email has been confirmed.
		/// </summary>
		public bool EmailConfirmed { get; set; } = false;

		/// <summary>
		/// The phone number of the user.
		/// </summary>
		public string PhoneNumber { get; set; } = string.Empty;

		/// <summary>
		/// Indicates whether the user's phone number has been confirmed.
		/// </summary>
		public bool PhoneNumberConfirmed { get; set; } = false;

		/// <summary>
		/// Hashed representation of the user's password.
		/// </summary>
		public string PasswordHash { get; set; } = string.Empty;

		/// <summary>
		/// Secret key used for additional security mechanisms.
		/// </summary>
		public string SecretKey { get; set; } = string.Empty;

		/// <summary>
		/// A token used to verify the integrity of security information.
		/// </summary>
		public string SecurityStamp { get; set; } = string.Empty;

		/// <summary>
		/// A token used to handle concurrency checks.
		/// </summary>
		public string ConcurrencyStamp { get; set; } = string.Empty;

		/// <summary>
		/// The user's preferred time zone (IANA or Windows time zone ID).
		/// </summary>
		public string TimeZoneId { get; set; } = string.Empty;

		/// <summary>
		/// The user's preferred currency (e.g., USD, VND).
		/// </summary>
		public string Currency { get; set; } = string.Empty;

		/// <summary>
		/// The user's preferred language code (e.g., en-US, vi-VN).
		/// </summary>
		public string Language { get; set; } = string.Empty;

		/// <summary>
		/// Indicates whether two-factor authentication is enabled.
		/// </summary>
		public bool TwoFactorEnabled { get; set; } = false;

		/// <summary>
		/// Number of consecutive failed login attempts.
		/// </summary>
		public int? AccessFailedCount { get; set; }

		/// <summary>
		/// Number of failed attempts for verification flows (e.g., OTP).
		/// </summary>
		public int? VerifyFailedCount { get; set; }

		/// <summary>
		/// Account status (e.g., New, Active, Inactive).
		/// Stored as an integer representing UserStatusEnum.
		/// </summary>
		public int Status { get; set; } = (int)UserStatusEnum.New;

		/// <summary>
		/// Realtime connection status (e.g., Online, Offline).
		/// Stored as an integer representing ConnectionStatusEnum.
		/// </summary>
		public int ConnectionStatus { get; set; } = (int)ConnectionStatusEnum.Offline;

		/// <summary>
		/// Reason why the user account is locked (if applicable).
		/// </summary>
		public string LockedReason { get; set; } = string.Empty;

		/// <summary>
		/// The user's date of birth.
		/// </summary>
		public DateTime? DateOfBirth { get; set; }

		/// <summary>
		/// The last time the user was seen online (UTC).
		/// </summary>
		public DateTime? LastSeenAt_Utc { get; set; }

		/// <summary>
		/// The date and time when the user is locked out until (UTC).
		/// </summary>
		public DateTime? LockoutEndDate_Utc { get; set; }

		/// <summary>
		/// ID reference to the user's avatar file.
		/// </summary>
		public Guid? AvatarId { get; set; }

		/// <summary>
		/// The user's avatar file entity.
		/// </summary>
		public virtual FileEntity? Avartar { get; set; }

		/// <summary>
		/// Roles assigned to the user.
		/// </summary>
		public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }

		/// <summary>
		/// Claims associated with the user.
		/// </summary>
		public virtual ICollection<ApplicationUserClaim>? UserClaims { get; set; }

		/// <summary>
		/// External login providers linked to the user.
		/// </summary>
		public virtual ICollection<ApplicationUserLogin>? UserLogins { get; set; }

		/// <summary>
		/// Tokens associated with the user (e.g., refresh tokens, 2FA tokens).
		/// </summary>
		public virtual ICollection<ApplicationUserToken>? UserTokens { get; set; }
	}
}
