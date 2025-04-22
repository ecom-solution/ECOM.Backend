namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a source from which currency exchange rates are retrieved.
    /// This entity stores information about the provider, API details, update frequency, and configuration.
    /// This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public class CurrencyExchangeRateSource : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CurrencyExchangeRateSource"/> class.
        /// </summary>
        public CurrencyExchangeRateSource() { }

        /// <summary>
        /// Gets or sets the descriptive name of the exchange rate source (e.g., "Open Exchange Rates", "CurrencyLayer").
        /// Defaults to an empty string.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the base URL of the API or the location to retrieve exchange rate data from.
        /// Defaults to an empty string.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the API key required to authenticate with the exchange rate source, if applicable.
        /// Defaults to an empty string.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the API secret required for authentication with the exchange rate source, if applicable.
        /// Defaults to an empty string.
        /// </summary>
        public string ApiSecret { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the interval in seconds at which the system should attempt to update exchange rates from this source.
        /// Defaults to 3600 seconds (1 hour).
        /// </summary>
        public int UpdateIntervalSeconds { get; set; } = 3600; // Default to 1 hour

        /// <summary>
        /// Gets or sets a boolean value indicating whether this exchange rate source is currently active and being used for updates.
        /// Defaults to <c>true</c>.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets a configuration string that can hold additional settings specific to this exchange rate source (e.g., parsing rules, specific parameters).
        /// This allows for flexibility in handling different API formats and requirements.
        /// Defaults to an empty string.
        /// </summary>
        public string Configuration { get; set; } = string.Empty;
    }
}