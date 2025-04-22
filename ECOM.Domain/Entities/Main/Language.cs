namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a supported language within the ECOM application.
    /// This entity stores information about the language code, name, and whether it's the default language.
    /// It also links to translations and an optional avatar.
    /// This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public class Language : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        public Language() { }

        /// <summary>
        /// Gets or sets the unique language code (e.g., "en", "fr", "vi").
        /// This code is typically a two-letter ISO 639-1 code.
        /// Defaults to an empty string.
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name of the language (e.g., "English", "Français", "Tiếng Việt").
        /// Defaults to an empty string.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a boolean value indicating whether this language is the default language for the application.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// Gets or sets the optional unique identifier of the avatar image associated with this language.
        /// This could be a flag or other visual representation of the language. Nullable.
        /// </summary>
        public Guid? AvatarId { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="FileEntity"/> representing the avatar image for this language. Nullable.
        /// </summary>
        public virtual FileEntity? Avatar { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="LanguageTranslation"/> entities.
        /// These entities store the translations of language keys into this specific language.
        /// </summary>
        public virtual ICollection<LanguageTranslation>? LanguageTranslations { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="LanguageTranslationEntity"/> entities.
        /// These entities store translations for specific fields of data entities in this language.
        /// </summary>
        public virtual ICollection<LanguageTranslationEntity>? LanguageTranslationEntities { get; set; }
    }
}