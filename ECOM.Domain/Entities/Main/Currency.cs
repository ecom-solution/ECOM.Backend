namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a currency supported by the ECOM application.
    /// This entity stores details such as the currency code, name, symbol, default status,
    /// and formatting rules for displaying currency amounts.
    /// This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public class Currency : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Currency"/> class.
        /// </summary>
        public Currency() { }

        /// <summary>
        /// Gets or sets the unique three-letter ISO 4217 code for the currency
        /// (e.g., "USD" for United States Dollar, "EUR" for Euro, "VND" for Vietnamese Dong).
        /// Defaults to an empty string.
        /// </summary>
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

        /// <summary>
        /// Gets or sets an integer indicating the position of the currency symbol relative to the amount.
        /// 0: Before the amount (e.g., $100).
        /// 1: After the amount (e.g., 100€).
        /// Defaults to 0 (before amount).
        /// </summary>
        public int CurrencySymbolPosition { get; set; } = 0;

        /// <summary>
        /// Gets or sets the number of decimal digits to display for amounts in this currency.
        /// Defaults to 2.
        /// </summary>
        public int DecimalDigits { get; set; } = 2;

        /// <summary>
        /// Gets or sets the character used to separate the integer part from the fractional part
        /// of amounts in this currency (e.g., "." or ","). Defaults to ".".
        /// </summary>
        public string DecimalSeparator { get; set; } = ".";

        /// <summary>
        /// Gets or sets the character used to group digits in the integer part of amounts in this currency
        /// (e.g., "," or "."). Defaults to ",".
        /// </summary>
        public string ThousandsSeparator { get; set; } = ",";
    }
}