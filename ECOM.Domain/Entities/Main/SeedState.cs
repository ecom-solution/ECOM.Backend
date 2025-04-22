namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents the state of a specific seed operation in the database.
    /// This entity is used to track when a particular seed was last run
    /// and the hash of the seed data at that time to avoid redundant seeding.
    /// </summary>
    public class SeedState : BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique name of the seed operation (e.g., "Currency", "ProductCategory").
        /// This is used to identify the specific seed being tracked.
        /// </summary>
        public string SeedName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hash of the seed data file that was last successfully applied.
        /// This is used to detect changes in the seed data.
        /// </summary>
        public string CurrentHash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the UTC date and time when the seed operation was last successfully completed.
        /// </summary>
        public DateTime LastSeededAtUtc { get; set; }

        /// <summary>
        /// Gets or sets the UTC date and time when the seed data file was last modified.
        /// This provides an additional way to detect potential changes.
        /// </summary>
        public DateTime? LastModifiedAtUtc { get; set; }
    }
}