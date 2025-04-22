namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a role within the ECOM application. Roles are used to group permissions
    /// and assign them to users, enabling role-based access control.
    /// This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public class ApplicationRole : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class.
        /// </summary>
        public ApplicationRole() { }

        /// <summary>
        /// Gets or sets the unique name of the role (e.g., "Administrator", "Editor", "Customer").
        /// This name is used to identify the role within the application.
        /// Defaults to an empty string.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional description providing more context or information about this role
        /// and the permissions it encompasses. Nullable.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="ApplicationUserRole"/> entities.
        /// These entities link users to this role.
        /// </summary>
        public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="ApplicationRoleClaim"/> entities.
        /// These entities define the specific claims (permissions) associated with this role.
        /// </summary>
        public virtual ICollection<ApplicationRoleClaim>? RoleClaims { get; set; }
    }
}