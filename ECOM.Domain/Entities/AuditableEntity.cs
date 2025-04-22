namespace ECOM.Domain.Entities
{
    /// <summary>
    /// Represents a base entity that includes auditing information, tracking who created and last updated the entity,
    /// as well as the timestamps for these actions. This class inherits from <see cref="BaseEntity"/>, inheriting its unique identifier.
    /// </summary>
    public abstract class AuditableEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who created the entity.
        /// Nullable, as the creator might not always be known or set explicitly.
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the entity was created.
        /// Defaults to the current UTC time when a new instance is created.
        /// </summary>
        public DateTime CreatedAt_Utc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the unique identifier of the user who last updated the entity.
        /// Nullable, as the entity might not have been updated after creation.
        /// </summary>
        public Guid? LastUpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the entity was last updated.
        /// Defaults to the current UTC time when a new instance is created, and will be updated
        /// whenever the entity is modified and saved.
        /// </summary>
        public DateTime LastUpdatedAt_Utc { get; set; } = DateTime.UtcNow;
    }
}