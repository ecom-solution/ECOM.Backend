using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationUserClaim"/> entity, defining its mapping to the database table
    /// and its composite primary key and relationships with <see cref="ApplicationUser"/> and <see cref="ApplicationClaim"/>.
    /// This class implements the <see cref="IEntityTypeConfiguration{ApplicationUserClaim}"/> interface.
    /// </summary>
    public class ApplicationUserClaimConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUserClaim>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationUserClaim"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            builder.ToTable(nameof(ApplicationUserClaim)); // Maps the entity to a table named "ApplicationUserClaim"

            // Configures the composite primary key consisting of UserId and ClaimId
            builder.HasKey(x => new { x.UserId, x.ClaimId });

            // Configures the many-to-many relationship between ApplicationUser and ApplicationClaim through ApplicationUserClaim
            // This configures the relationship with the ApplicationUser entity
            builder.HasOne(x => x.User) // ApplicationUserClaim has one ApplicationUser
                   .WithMany(x => x.UserClaims) // ApplicationUser has many ApplicationUserClaims
                   .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserClaims are also deleted

            // Configures the relationship with the ApplicationClaim entity
            builder.HasOne(x => x.Claim) // ApplicationUserClaim has one ApplicationClaim
                   .WithMany(x => x.UserClaims) // ApplicationClaim has many ApplicationUserClaims
                   .HasForeignKey(x => x.ClaimId) // Defines the foreign key "ClaimId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationClaim is deleted, all associated ApplicationUserClaims are also deleted
        }
    }
}