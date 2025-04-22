using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="CurrencyExchangeRate"/> entity, defining its mapping to the database table
    /// and its relationships with other entities. This class implements the <see cref="IEntityTypeConfiguration{CurrencyExchangeRate}"/> interface.
    /// </summary>
    public class CurrencyExchangeRateConfiguration : MainConfiguration, IEntityTypeConfiguration<CurrencyExchangeRate>
    {
        /// <summary>
        /// Configures the <see cref="CurrencyExchangeRate"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<CurrencyExchangeRate> builder)
        {
            builder.ToTable(nameof(CurrencyExchangeRate)); // Maps the entity to a table named "CurrencyExchangeRate"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key

            builder.Property(x => x.Rate).HasColumnType("decimal(18, 6)").IsRequired(); // Configures the "Rate" property as a decimal with high precision and required
            builder.Property(x => x.LastUpdatedDate_Utc).IsRequired(); // Configures the "LastUpdatedDate_Utc" property as required

            // Configures the foreign key and relationship with the CurrencyExchangeRateSource entity
            builder.HasOne(x => x.CurrencyExchangeRateSource)
                   .WithMany() // A CurrencyExchangeRateSource can have many CurrencyExchangeRates
                   .HasForeignKey(x => x.CurrencyExchangeRateSourceId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevents deletion of a CurrencyExchangeRateSource if there are associated CurrencyExchangeRates

            // Configures the foreign key and relationship with the FromCurrency entity
            builder.HasOne(x => x.FromCurrency)
                   .WithMany() // A Currency can be the source currency in many exchange rates
                   .HasForeignKey(x => x.FromCurrencyId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevents deletion of a Currency if it's used as a source currency

            // Configures the foreign key and relationship with the ToCurrency entity
            builder.HasOne(x => x.ToCurrency)
                   .WithMany() // A Currency can be the target currency in many exchange rates
                   .HasForeignKey(x => x.ToCurrencyId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevents deletion of a Currency if it's used as a target currency

            // Creates an index on the foreign key columns for better query performance
            builder.HasIndex(x => x.CurrencyExchangeRateSourceId);
            builder.HasIndex(x => x.FromCurrencyId);
            builder.HasIndex(x => x.ToCurrencyId);

            // Creates a unique index on the combination of FromCurrencyId and ToCurrencyId
            // to prevent duplicate exchange rates for the same currency pair
            builder.HasIndex(x => new { x.FromCurrencyId, x.ToCurrencyId }).IsUnique();
        }
    }
}