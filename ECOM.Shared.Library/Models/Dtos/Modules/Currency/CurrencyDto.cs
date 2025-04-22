namespace ECOM.Shared.Library.Models.Dtos.Modules.Currency
{
    public class CurrencyDto
    {
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name of the currency
        /// (e.g., "United States Dollar", "Euro", "Vietnamese Dong").
        /// Defaults to an empty string.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the symbol used to represent the currency
        /// (e.g., "$" for Dollar, "€" for Euro, "₫" for Dong).
        /// Defaults to an empty string.
        /// </summary>
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a boolean value indicating whether this currency is the default currency
        /// for the ECOM system. Defaults to <c>false</c>.
        /// </summary>
        public bool IsDefault { get; set; } = false;
    }
}
