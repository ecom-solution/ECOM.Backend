using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="ApplicationUserLogin"/> entity, defining its mapping to the database table,
    /// composite primary key, and relationship with the <see cref="ApplicationUser"/> entity.
    /// This class implements the <see cref="IEntityTypeConfiguration{ApplicationUserLogin}"/> interface.
    /// </summary>
    public class ApplicationUserLoginConfiguration : MainConfiguration, IEntityTypeConfiguration<ApplicationUserLogin>
    {
        /// <summary>
        /// Configures the <see cref="ApplicationUserLogin"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            builder.ToTable(nameof(ApplicationUserLogin)); // Maps the entity to a table named "ApplicationUserLogin"

            // Configures the composite primary key consisting of Provider and ProviderKey
            builder.HasKey(x => new { x.Provider, x.ProviderKey });

            builder.Property(x => x.Provider).HasMaxLength(100).IsRequired(); // Configures the "Provider" property: maximum length 100 and required
            builder.Property(x => x.ProviderKey).HasMaxLength(100).IsRequired(); // Configures the "ProviderKey" property: maximum length 100 and required

            // Configures the one-to-many relationship between ApplicationUserLogin and ApplicationUser
            builder.HasOne(x => x.User) // ApplicationUserLogin belongs to one ApplicationUser
                   .WithMany(x => x.UserLogins) // ApplicationUser has many ApplicationUserLogins
                   .HasForeignKey(x => x.UserId) // Defines the foreign key "UserId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when an ApplicationUser is deleted, all associated ApplicationUserLogins are also deleted
        }
    }
}