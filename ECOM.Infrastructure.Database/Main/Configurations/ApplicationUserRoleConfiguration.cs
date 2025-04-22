using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationUserRole"/> entity, defining its mapping to the database table,
    /// composite primary key, and relationships with <see cref="ApplicationUser"/> and <see cref="ApplicationRole"/>.
    /// This class implements the <see cref="IEntityTypeConfiguration{ApplicationUserRole}"/> interface.
    /// </summary>
    public class ApplicationUserRoleConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUserRole>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationUserRole"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable(nameof(ApplicationUserRole)); // Maps the entity to a table named "ApplicationUserRole"

            // Configures the composite primary key consisting of UserId and RoleId
            builder.HasKey(x => new { x.UserId, x.RoleId });

            // Configures the many-to-many relationship between ApplicationUser and ApplicationRole through ApplicationUserRole
            // This configures the relationship with the ApplicationUser entity
            builder.HasOne(x => x.User) // ApplicationUserRole belongs to one ApplicationUser
                   .WithMany(x => x.UserRoles) // ApplicationUser has many ApplicationUserRoles
                   .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserRoles are also deleted

            // Configures the relationship with the ApplicationRole entity
            builder.HasOne(x => x.Role) // ApplicationUserRole belongs to one ApplicationRole
                   .WithMany(x => x.UserRoles) // ApplicationRole has many ApplicationUserRoles
                   .HasForeignKey(x => x.RoleId) // Defines the foreign key "RoleId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationRole is deleted, all associated ApplicationUserRoles are also deleted
        }
    }
}