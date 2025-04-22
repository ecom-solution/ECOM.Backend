namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents the link between an <see cref="ApplicationUser"/> and an <see cref="ApplicationRole"/>.
    /// This entity defines which roles are assigned to which users, enabling role-based access control.
    /// </summary>
    public class ApplicationUserRole
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserRole"/> class.
        /// </summary>
        public ApplicationUserRole() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserRole"/> class with specific user and role identifiers.
        /// </summary>
        /// <param name="userId">The unique identifier of the <see cref="ApplicationUser"/>.</param>
        /// <param name="roleId">The unique identifier of the <see cref="ApplicationRole"/>.</param>
        public ApplicationUserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="ApplicationUser"/> associated with this role assignment.
        /// This is a foreign key linking to the user.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="ApplicationRole"/> assigned to the user.
        /// This is a foreign key linking to the role.
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ApplicationUser"/> entity.
        /// </summary>
        public virtual ApplicationUser? User { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="ApplicationRole"/> entity.
        /// </summary>
        public virtual ApplicationRole? Role { get; set; }
    }
}