using ECOM.Shared.Library.Enums.Entity;

namespace ECOM.Domain.Entities.Main
{
	public class ApplicationUser : AuditableEntity
    {
        public ApplicationUser() { }

		public string UserName { get; set; } = string.Empty;

		public string NormalizedUserName { get; set; } = string.Empty;

		public string FullName { get; set; } = string.Empty;

		public string NormalizedFullName { get; set; } = string.Empty;

		public string Email { get; set; } = string.Empty;

		public string NormalizedEmail { get; set; } = string.Empty;

		public bool EmailConfirmed { get; set; } = false;

		public string PhoneNumber { get; set; } = string.Empty;

		public bool PhoneNumberConfirmed { get; set; } = false;

		public string PasswordHash { get; set; } = string.Empty;

		public string SecretKey { get; set; } = string.Empty;

		public string SecurityStamp { get; set; } = string.Empty;

		public string ConcurrencyStamp { get; set; } = string.Empty;

		public string TimeZoneId { get; set; } = string.Empty;

		public string Currency { get; set; } = string.Empty;

		public string Language { get; set; } = string.Empty;

		public bool TwoFactorEnabled { get; set; } = false;

		public int? AccessFailedCount { get; set; }

		public int? VerifyFailedCount { get; set; }

		public int Status { get; set; } = (int)UserStatus.New;

		public string LockedReason { get; set; } = string.Empty;

		public DateTime? DateOfBirth { get; set; }

		public DateTime? LockoutEndDate_Utc { get; set; }

		public Guid? AvatarId { get; set; }

		public virtual FileEntity? Avartar { get; set; }
		public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
		public virtual ICollection<ApplicationUserClaim>? UserClaims { get; set; }
		public virtual ICollection<ApplicationUserLogin>? UserLogins { get; set; }
		public virtual ICollection<ApplicationUserToken>? UserTokens { get; set; }
	}
}
