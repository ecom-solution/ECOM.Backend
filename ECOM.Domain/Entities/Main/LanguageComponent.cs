namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a logical grouping or category for language keys (<see cref="LanguageKey"/>).
    /// This helps organize and manage translations within the application, often based on features or modules.
    /// This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public class LanguageComponent : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageComponent"/> class.
        /// </summary>
        public LanguageComponent() { }

        /// <summary>
        /// Gets or sets the unique identifier of the parent <see cref="LanguageComponent"/>, allowing for a hierarchical structure of components.
        /// Nullable, indicating that this component is a root-level component.
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the unique name of this language component (e.g., "ProductDetails", "Checkout").
        /// This name is used to identify and group related language keys.
        /// Defaults to an empty string.
        /// </summary>
        public string ComponentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional description providing more context or information about this language component.
        /// Nullable.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Navigation property to the parent <see cref="LanguageComponent"/> entity.
        /// </summary>
        public virtual LanguageComponent? Parent { get; set; }

        /// <summary>
        /// Navigation property to the collection of child <see cref="LanguageComponent"/> entities, forming a tree structure.
        /// </summary>
        public virtual ICollection<LanguageComponent>? Children { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="LanguageKey"/> entities that belong to this language component.
        /// </summary>
        public virtual ICollection<LanguageKey>? LanguageKeys { get; set; }
    }
}