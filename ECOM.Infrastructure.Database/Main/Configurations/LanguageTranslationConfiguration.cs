using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="LanguageTranslation"/> entity, defining its mapping to the database table,
    /// composite primary key, properties related to translated text, and its relationships with <see cref="Language"/> and <see cref="LanguageKey"/>.
    /// This class implements the <see cref="IEntityTypeConfiguration{LanguageTranslation}"/> interface.
    /// </summary>
    public class LanguageTranslationConfiguration : MainConfiguration, IEntityTypeConfiguration<LanguageTranslation>
    {
        /// <summary>
        /// Configures the <see cref="LanguageTranslation"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<LanguageTranslation> builder)
        {
            builder.ToTable(nameof(LanguageTranslation)); // Maps the entity to a table named "LanguageTranslation"

            // Configures the composite primary key consisting of LanguageId and LanguageKeyId
            builder.HasKey(x => new { x.LanguageId, x.LanguageKeyId });

            builder.Property(x => x.Value).HasMaxLength(500).IsRequired(); // Configures the "Value" property: maximum length 500 and required (the translated text)

            // Configures the many-to-many relationship between Language and LanguageKey through LanguageTranslation
            // This configures the relationship with the Language entity
            builder.HasOne(x => x.Language) // LanguageTranslation belongs to one Language
                   .WithMany(x => x.LanguageTranslations) // Language has many LanguageTranslations
                   .HasForeignKey(x => x.LanguageId) // Defines the foreign key "LanguageId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a Language is deleted, all associated LanguageTranslations are also deleted

            // Configures the relationship with the LanguageKey entity
            builder.HasOne(x => x.LanguageKey) // LanguageTranslation belongs to one LanguageKey
                   .WithMany(x => x.LanguageTranslations) // LanguageKey has many LanguageTranslations
                   .HasForeignKey(x => x.LanguageKeyId) // Defines the foreign key "LanguageKeyId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a LanguageKey is deleted, all associated LanguageTranslations are also deleted
        }
    }
}