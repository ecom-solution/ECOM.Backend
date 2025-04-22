using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationUserToken"/> entity, defining its mapping to the database table,
    /// composite primary key, properties, and relationship with the <see cref="ApplicationUser"/> entity.
    /// This class implements the <see cref="IEntityTypeConfiguration{ApplicationUserToken}"/> interface.
    /// </summary>
    public class ApplicationUserTokenConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUserToken>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationUserToken"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationUserToken> builder)
        {
            builder.ToTable(nameof(ApplicationUserToken)); // Maps the entity to a table named "ApplicationUserToken"

            // Configures the composite primary key consisting of UserId, Provider, and TokenName
            builder.HasKey(x => new { x.UserId, x.Provider, x.TokenName });

            builder.Property(x => x.Provider).HasMaxLength(100).IsRequired(); // Configures the "Provider" property: maximum length 100 and required
            builder.Property(x => x.TokenName).HasMaxLength(100).IsRequired(); // Configures the "TokenName" property: maximum length 100 and required
            builder.Property(x => x.TokenValue).HasMaxLength(int.MaxValue).IsRequired(); // Configures the "TokenValue" property: maximum length unlimited and required
            builder.Property(x => x.TokenExpiredAt_Utc).IsRequired(); // Configures the "TokenExpiredAt_Utc" property as required

            // Configures the one-to-many relationship between ApplicationUserToken and ApplicationUser
            builder.HasOne(x => x.User) // ApplicationUserToken belongs to one ApplicationUser
                   .WithMany(x => x.UserTokens) // ApplicationUser has many ApplicationUserTokens
                   .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserTokens are also deleted
        }
    }
}