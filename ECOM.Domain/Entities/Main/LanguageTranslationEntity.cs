namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a translation for a specific field of a specific entity in a particular language.
    /// This allows for multi-language support for dynamic data within the application.
    /// </summary>
    public class LanguageTranslationEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslationEntity"/> class.
        /// </summary>
        public LanguageTranslationEntity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageTranslationEntity"/> class with specific language, entity, and ID.
        /// </summary>
        /// <param name="languageId">The unique identifier of the language for this translation.</param>
        /// <param name="entityName">The name of the entity being translated (e.g., "Product", "Category").</param>
        /// <param name="entityId">The unique identifier of the specific entity instance being translated.</param>
        public LanguageTranslationEntity(Guid languageId, string entityName, Guid entityId)
        {
            LanguageId = languageId;
            EntityName = entityName;
            EntityId = entityId;
        }

        /// <summary>
        /// Gets or sets the unique identifier of the language for this translation, referencing the <see cref="Language"/> entity.
        /// </summary>
        public Guid LanguageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity whose field is being translated (e.g., "Product", "Category").
        /// Defaults to an empty string.
        /// </summary>
        public string EntityName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the specific instance of the entity being translated.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Gets or sets the name of the specific field of the entity that is being translated (e.g., "Name", "Description").
        /// Defaults to an empty string.
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the translated value for the specified field of the entity in the specified language.
        /// Defaults to an empty string.
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the <see cref="Language"/> entity, representing the language of this translation.
        /// </summary>
        public virtual Language? Language { get; set; }
    }
}