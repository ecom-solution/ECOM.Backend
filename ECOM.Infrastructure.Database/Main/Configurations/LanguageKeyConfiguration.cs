using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="LanguageKey"/> entity, defining its mapping to the database table,
    /// properties related to translation keys, and its relationships with <see cref="LanguageComponent"/> and <see cref="LanguageTranslation"/>.
    /// This class implements the <see cref="IEntityTypeConfiguration{LanguageKey}"/> interface.
    /// </summary>
    public class LanguageKeyConfiguration : MainConfiguration, IEntityTypeConfiguration<LanguageKey>
    {
        /// <summary>
        /// Configures the <see cref="LanguageKey"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<LanguageKey> builder)
        {
            builder.ToTable(nameof(LanguageKey)); // Maps the entity to a table named "LanguageKey"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key

            builder.Property(x => x.Key).HasMaxLength(500).IsRequired(); // Configures the "Key" property: maximum length 500 and required (the unique identifier for a translatable string)
            builder.Property(x => x.Description).HasMaxLength(500); // Configures the "Description" property: maximum length 500 (optional context for the key)

            // Configures the one-to-many relationship between LanguageKey and LanguageTranslation
            builder.HasMany(x => x.LanguageTranslations) // LanguageKey has many LanguageTranslations
                .WithOne(x => x.LanguageKey) // Each LanguageTranslation belongs to one LanguageKey
                .HasForeignKey(x => x.LanguageKeyId) // Defines the foreign key "LanguageKeyId" in the LanguageTranslation table
                .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a LanguageKey is deleted, all associated LanguageTranslations are also deleted
        }
    }
}