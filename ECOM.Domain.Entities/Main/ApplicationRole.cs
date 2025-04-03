namespace ECOM.Domain.Entities.Main
{
	public class ApplicationRole : BaseEntity
    {
        public ApplicationRole() { }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

		public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
		public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }
	}
}
