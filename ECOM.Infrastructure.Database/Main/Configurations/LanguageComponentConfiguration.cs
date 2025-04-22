using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="LanguageComponent"/> entity, defining its mapping to the database table,
    /// properties related to language components (grouping of language keys), and its relationships with other entities.
    /// This class implements the <see cref="IEntityTypeConfiguration{LanguageComponent}"/> interface.
    /// </summary>
    public class LanguageComponentConfiguration : MainConfiguration, IEntityTypeConfiguration<LanguageComponent>
    {
        /// <summary>
        /// Configures the <see cref="LanguageComponent"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<LanguageComponent> builder)
        {
            builder.ToTable(nameof(LanguageComponent)); // Maps the entity to a table named "LanguageComponent"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key
            builder.HasIndex(x => x.ComponentName).IsUnique(); // Creates a unique index on the "ComponentName" property

            builder.Property(x => x.ComponentName).HasMaxLength(255).IsUnicode(false).IsRequired(); // Configures the "ComponentName" property: maximum length 255, non-Unicode, and required
            builder.Property(x => x.Description).HasMaxLength(500); // Configures the "Description" property: maximum length 500

            // Configures the self-referencing one-to-many relationship for hierarchical language components
            builder.HasOne(x => x.Parent) // LanguageComponent has one parent LanguageComponent (nullable)
                .WithMany(x => x.Children) // Parent LanguageComponent can have many child LanguageComponents
                .HasForeignKey(x => x.ParentId) // Defines the foreign key "ParentId"
                .OnDelete(DeleteBehavior.NoAction); // Configures the delete behavior: prevents deletion if there are child components

            // Configures the one-to-many relationship between LanguageComponent and LanguageKey
            builder.HasMany(x => x.LanguageKeys) // LanguageComponent has many LanguageKeys
                .WithOne(x => x.LanguageComponent) // Each LanguageKey belongs to one LanguageComponent
                .HasForeignKey(x => x.LanguageComponentId) // Defines the foreign key "LanguageComponentId" in the LanguageKey table
                .OnDelete(DeleteBehavior.Restrict); // Configures the delete behavior: prevents deletion if there are associated LanguageKeys
        }
    }
}