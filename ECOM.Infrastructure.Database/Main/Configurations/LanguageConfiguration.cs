using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="Language"/> entity, defining its mapping to the database table,
    /// properties related to supported languages, and its relationships with other entities.
    /// This class implements the <see cref="IEntityTypeConfiguration{Language}"/> interface.
    /// </summary>
    public class LanguageConfiguration : MainConfiguration, IEntityTypeConfiguration<Language>
    {
        /// <summary>
        /// Configures the <see cref="Language"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.ToTable(nameof(Language)); // Maps the entity to a table named "Language"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key
            builder.HasIndex(x => x.Code).IsUnique(); // Creates a unique index on the "Code" property

            builder.Property(x => x.Code).HasMaxLength(2).IsUnicode(false).IsRequired(); // Configures the "Code" property: maximum length 2, non-Unicode, and required (e.g., "en", "vi")
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired(); // Configures the "Name" property: maximum length 50 and required (e.g., "English", "Vietnamese")
            builder.Property(x => x.IsDefault).HasDefaultValue(false).IsRequired(); // Configures the "IsDefault" property: default value false and required

            // Configures the one-to-many relationship between Language and LanguageTranslation
            builder.HasMany(x => x.LanguageTranslations) // Language has many LanguageTranslations
                .WithOne(x => x.Language) // Each LanguageTranslation belongs to one Language
                .HasForeignKey(x => x.LanguageId) // Defines the foreign key "LanguageId" in the LanguageTranslation table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a Language is deleted, all associated LanguageTranslations are also deleted
        }
    }
}