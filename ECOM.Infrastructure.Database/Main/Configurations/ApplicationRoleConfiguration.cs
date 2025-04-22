using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationRole"/> entity, defining its mapping to the database table
    /// and its relationships with other entities. This class implements the <see cref="IEntityTypeConfiguration{ApplicationRole}"/> interface.
    /// </summary>
    public class ApplicationRoleConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationRole>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationRole"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.ToTable(nameof(ApplicationRole)); // Maps the entity to a table named "ApplicationRole"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key
            builder.Property(x => x.Name).HasMaxLength(250).IsRequired(); // Configures the "Name" property: maximum length 250 and required
            builder.Property(x => x.Description).HasMaxLength(500); // Configures the "Description" property: maximum length 500

            // Configures the one-to-many relationship between ApplicationRole and ApplicationRoleClaim
            builder.HasMany(x => x.RoleClaims) // ApplicationRole has many ApplicationRoleClaims
                   .WithOne(x => x.Role) // Each ApplicationRoleClaim has one ApplicationRole
                   .HasForeignKey(x => x.RoleId) // Defines the foreign key "RoleId" in the ApplicationRoleClaim table
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationRole is deleted, all associated ApplicationRoleClaims are also deleted
        }
    }
}