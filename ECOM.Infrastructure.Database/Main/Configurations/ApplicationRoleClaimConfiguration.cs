using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationRoleClaim"/> entity, defining its mapping to the database table
    /// and its composite primary key and relationships with <see cref="ApplicationRole"/> and <see cref="ApplicationClaim"/>.
    /// This class implements the <see cref="IEntityTypeConfiguration{ApplicationRoleClaim}"/> interface.
    /// </summary>
    public class ApplicationRoleClaimConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationRoleClaim"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            builder.ToTable(nameof(ApplicationRoleClaim)); // Maps the entity to a table named "ApplicationRoleClaim"

            // Configures the composite primary key consisting of RoleId and ClaimId
            builder.HasKey(x => new { x.RoleId, x.ClaimId });

            // Configures the many-to-many relationship between ApplicationRole and ApplicationClaim through ApplicationRoleClaim
            // This configures the relationship with the ApplicationRole entity
            builder.HasOne(x => x.Role) // ApplicationRoleClaim has one ApplicationRole
                   .WithMany(x => x.RoleClaims) // ApplicationRole has many ApplicationRoleClaims
                   .HasForeignKey(x => x.RoleId) // Defines the foreign key "RoleId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationRole is deleted, all associated ApplicationRoleClaims are also deleted

            // Configures the relationship with the ApplicationClaim entity
            builder.HasOne(x => x.Claim) // ApplicationRoleClaim has one ApplicationClaim
                   .WithMany(x => x.RoleClaims) // ApplicationClaim has many ApplicationRoleClaims
                   .HasForeignKey(x => x.ClaimId) // Defines the foreign key "ClaimId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationClaim is deleted, all associated ApplicationRoleClaims are also deleted
        }
    }
}