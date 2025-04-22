using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="LanguageTranslationEntity"/> entity, defining its mapping to the database table,
    /// composite primary key, properties related to translated entity fields, and its relationship with the <see cref="Language"/> entity.
    /// This class implements the <see cref="IEntityTypeConfiguration{LanguageTranslationEntity}"/> interface.
    /// </summary>
    public class LanguageTranslationEntityConfiguration : MainConfiguration, IEntityTypeConfiguration<LanguageTranslationEntity>
    {
        /// <summary>
        /// Configures the <see cref="LanguageTranslationEntity"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<LanguageTranslationEntity> builder)
        {
            builder.ToTable(nameof(LanguageTranslationEntity)); // Maps the entity to a table named "LanguageTranslationEntity"

            // Configures the composite primary key consisting of LanguageId, EntityName, EntityId, and FieldName
            builder.HasKey(x => new { x.LanguageId, x.EntityName, x.EntityId, x.FieldName });

            builder.Property(x => x.EntityName).HasMaxLength(100).IsRequired(); // Configures the "EntityName" property: maximum length 100 and required (e.g., "Product", "Category")
            builder.Property(x => x.FieldName).HasMaxLength(100).IsRequired(); // Configures the "FieldName" property: maximum length 100 and required (e.g., "Name", "Description")
            builder.Property(x => x.Value).HasMaxLength(5000).IsRequired(); // Configures the "Value" property: maximum length 5000 and required (the translated value of the field)

            // Configures the one-to-many relationship between LanguageTranslationEntity and Language
            builder.HasOne(x => x.Language) // LanguageTranslationEntity belongs to one Language
                   .WithMany(x => x.LanguageTranslationEntities) // Language has many LanguageTranslationEntities
                   .HasForeignKey(x => x.LanguageId) // Defines the foreign key "LanguageId"
                   .OnDelete(DeleteBehavior.Cascade); // Configures the delete behavior: when a Language is deleted, all associated LanguageTranslationEntities are also deleted
        }
    }
}