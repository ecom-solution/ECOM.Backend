using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="Currency"/> entity, defining its mapping to the database table
    /// and any specific constraints or relationships. This class implements the <see cref="IEntityTypeConfiguration{Currency}"/> interface.
    /// </summary>
    public class CurrencyConfiguration : MainConfiguration, IEntityTypeConfiguration<Currency>
    {
        /// <summary>
        /// Configures the <see cref="Currency"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable(nameof(Currency)); // Maps the entity to a table named "Currency"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key
            builder.HasIndex(x => x.Code).IsUnique(); // Creates a unique index on the "Code" property

            builder.Property(x => x.Code).HasMaxLength(10).IsUnicode(false).IsRequired(); // Configures the "Code" property: maximum length 10, non-Unicode, and required
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired(); // Configures the "Name" property: maximum length 100 and required
            builder.Property(x => x.Symbol).HasMaxLength(10).IsRequired(); // Configures the "Symbol" property: maximum length 10 and required
            builder.Property(x => x.IsDefault).HasDefaultValue(false).IsRequired(); // Configures the "IsDefault" property: default value false and required
            builder.Property(x => x.CurrencySymbolPosition).HasDefaultValue(0).IsRequired(); // Configures the "CurrencySymbolPosition" property: default value 0 and required
            builder.Property(x => x.DecimalDigits).HasDefaultValue(2).IsRequired(); // Configures the "DecimalDigits" property: default value 2 and required
            builder.Property(x => x.DecimalSeparator).HasMaxLength(5).IsRequired(); // Configures the "DecimalSeparator" property: maximum length 5 and required
            builder.Property(x => x.ThousandsSeparator).HasMaxLength(5).IsRequired(); // Configures the "ThousandsSeparator" property: maximum length 5 and required

            // No complex relationships are configured here for the Currency entity in this example.
            // If Currency had relationships with other entities, they would be configured here using methods like HasMany, HasOne, WithMany, etc.
        }
    }
}