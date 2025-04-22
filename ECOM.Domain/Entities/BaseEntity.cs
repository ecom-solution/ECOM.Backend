using ECOM.Shared.Library.Functions.Helpers;

namespace ECOM.Domain.Entities
{
    /// <summary>
    /// Represents a base entity with a unique identifier.
    /// All entities in the domain should inherit from this class to ensure a consistent primary key.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// The default value is generated using the <see cref="CommonHelper.GenerateSequenceGuid"/> method
        /// to provide a sequential GUID, which can be beneficial for database performance in some scenarios.
        /// </summary>
        public Guid Id { get; set; } = CommonHelper.GenerateSequenceGuid();
    }
}