namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a unique key used for identifying translatable text within the application.
    /// These keys are typically grouped under <see cref="LanguageComponent"/> for better organization.
    /// This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public class LanguageKey : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageKey"/> class.
        /// </summary>
        public LanguageKey() { }

        /// <summary>
        /// Gets or sets the unique key string used to identify this translatable text (e.g., "product.name", "button.submit").
        /// This key is used to retrieve the appropriate translation for a given language.
        /// Defaults to an empty string.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional description or context for this language key.
        /// This can provide additional information to translators about where and how this text is used.
        /// Nullable.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="LanguageTranslation"/> entities associated with this language key.
        /// Each <see cref="LanguageTranslation"/> represents the translation of this key in a specific language.
        /// </summary>
        public virtual ICollection<LanguageTranslation>? LanguageTranslations { get; set; }
    }
}