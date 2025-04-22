using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="CurrencyExchangeRateSource"/> entity, defining its mapping to the database table
    /// and any specific constraints or relationships. This class implements the <see cref="IEntityTypeConfiguration{CurrencyExchangeRateSource}"/> interface.
    /// </summary>
    public class CurrencyExchangeRateSourceConfiguration : MainConfiguration, IEntityTypeConfiguration<CurrencyExchangeRateSource>
    {
        /// <summary>
        /// Configures the <see cref="CurrencyExchangeRateSource"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<CurrencyExchangeRateSource> builder)
        {
            builder.ToTable(nameof(CurrencyExchangeRateSource)); // Maps the entity to a table named "CurrencyExchangeRateSource"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired(); // Configures the "Name" property: maximum length 200 and required
            builder.Property(x => x.Url).HasMaxLength(500); // Configures the "Url" property: maximum length 500
            builder.Property(x => x.ApiKey).HasMaxLength(200); // Configures the "ApiKey" property: maximum length 200
            builder.Property(x => x.ApiSecret).HasMaxLength(200); // Configures the "ApiSecret" property: maximum length 200
            builder.Property(x => x.UpdateIntervalSeconds).HasDefaultValue(3600).IsRequired(); // Configures the "UpdateIntervalSeconds" property: default value 3600 and required
            builder.Property(x => x.IsActive).HasDefaultValue(true).IsRequired(); // Configures the "IsActive" property: default value true and required
            builder.Property(x => x.Configuration).HasColumnType("nvarchar(max)"); // Configures the "Configuration" property as a long Unicode string
        }
    }
}