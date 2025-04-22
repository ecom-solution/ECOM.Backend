namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a specific currency exchange rate between two currencies,
    /// including the rate value and the source from which it was obtained.
    /// This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public class CurrencyExchangeRate : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyExchangeRate"/> class.
        /// </summary>
        public CurrencyExchangeRate() { }

        /// <summary>
        /// Gets or sets the unique identifier of the <see cref="CurrencyExchangeRateSource"/>
        /// that provided this exchange rate. This indicates the origin of the rate data.
        /// </summary>
        public Guid CurrencyExchangeRateSourceId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the source currency (<see cref="Currency"/>).
        /// This is the currency being converted from.
        /// </summary>
        public Guid FromCurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the target currency (<see cref="Currency"/>).
        /// This is the currency being converted to.
        /// </summary>
        public Guid ToCurrencyId { get; set; }

        /// <summary>
        /// Gets or sets the exchange rate value, representing the amount of the target currency
        /// equivalent to one unit of the source currency. Defaults to 0.
        /// </summary>
        public decimal Rate { get; set; } = 0;

        /// <summary>
        /// Gets or sets the UTC date and time when this exchange rate was last updated.
        /// This timestamp indicates the freshness of the exchange rate data.
        /// Defaults to the current UTC time.
        /// </summary>
        public DateTime LastUpdatedDate_Utc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to the <see cref="CurrencyExchangeRateSource"/> entity
        /// that this exchange rate belongs to. Can be null.
        /// </summary>
        public virtual CurrencyExchangeRateSource? CurrencyExchangeRateSource { get; set; }

        /// <summary>
        /// Navigation property to the source <see cref="Currency"/> entity. Can be null.
        /// </summary>
        public virtual Currency? FromCurrency { get; set; }

        /// <summary>
        /// Navigation property to the target <see cref="Currency"/> entity. Can be null.
        /// </summary>
        public virtual Currency? ToCurrency { get; set; }
    }
}