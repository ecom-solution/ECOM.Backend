namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a specific translation of a language key into a particular language.
    /// This entity links a <see cref="LanguageKey"/> with its translated <see cref="Value"/> in a specific <see cref="Language"/>.
    /// </summary>
    public class LanguageTranslation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslation"/> class.
        /// </summary>
        public LanguageTranslation() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslation"/> class with specific language and language key identifiers.
        /// </summary>
        /// <param name="languageId">The unique identifier of the language for this translation.</param>
        /// <param name="languageKeyId">The unique identifier of the language key being translated.</param>
        public LanguageTranslation(Guid languageId, Guid languageKeyId)
        {
            LanguageId = languageId;
            LanguageKeyId = languageKeyId;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the language for this translation, referencing the <see cref="Language"/> entity.
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the language key being translated, referencing the <see cref="LanguageKey"/> entity.
        /// </summary>
        public Guid LanguageKeyId { get; set; }

        /// <summary>
        /// Gets or sets the translated value of the <see cref="LanguageKey"/> in the specified <see cref="Language"/>.
        /// Defaults to an empty string.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the <see cref="Language"/> entity, representing the language of this translation.
        /// </summary>
        public virtual Language? Language { get; set; }

        /// <summary>
        /// Navigation property to the <see cref="LanguageKey"/> entity, representing the key being translated.
        /// </summary>
        public virtual LanguageKey? LanguageKey { get; set; }
    }
}