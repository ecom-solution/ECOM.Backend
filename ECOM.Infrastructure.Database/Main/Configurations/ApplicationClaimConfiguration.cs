using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationClaim"/> entity, defining its mapping to the database table
    /// and its relationships with other entities. This class implements the <see cref="IEntityTypeConfiguration{ApplicationClaim}"/> interface.
    /// </summary>
    public class ApplicationClaimConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationClaim>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationClaim"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationClaim> builder)
        {
            builder.ToTable(nameof(ApplicationClaim)); // Maps the entity to a table named "ApplicationClaim"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key

            builder.Property(x => x.ClaimType).HasMaxLength(250).IsRequired(); // Configures the "ClaimType" property: maximum length 250 and required
            builder.Property(x => x.ClaimValue).HasMaxLength(250).IsRequired(); // Configures the "ClaimValue" property: maximum length 250 and required
            builder.Property(x => x.Description).HasMaxLength(500); // Configures the "Description" property: maximum length 500

            // Configures the one-to-many relationship between ApplicationClaim and ApplicationRoleClaim
            builder.HasMany(x => x.RoleClaims) // ApplicationClaim has many ApplicationRoleClaims
                   .WithOne(x => x.Claim) // Each ApplicationRoleClaim has one ApplicationClaim
                   .HasForeignKey(x => x.ClaimId) // Defines the foreign key "ClaimId" in the ApplicationRoleClaim table
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationClaim is deleted, all associated ApplicationRoleClaims are also deleted
        }
    }
}