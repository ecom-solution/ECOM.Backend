namespace ECOM.Domain.Entities.Main
{
    /// <summary>
    /// Represents a file stored within the ECOM system, containing metadata about the file
    /// such as its name, URL, content type, size, and the storage bucket it resides in.
    /// This class inherits from <see cref="AuditableEntity"/>, providing common auditing properties.
    /// </summary>
    public class FileEntity : AuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileEntity"/> class.
        /// </summary>
        public FileEntity() { }

        /// <summary>
        /// Gets or sets the original name of the file.
        /// Defaults to an empty string.
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the URL or path where the file can be accessed.
        /// Defaults to an empty string.
        /// </summary>
        public string FileUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the MIME content type of the file (e.g., "image/jpeg", "application/pdf").
        /// Defaults to an empty string.
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the size of the file in bytes.
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the name of the storage bucket where the file is located (e.g., AWS S3 bucket name).
        /// Defaults to an empty string.
        /// </summary>
        public string BucketName { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the collection of <see cref="ApplicationUser"/> entities that use this file as their avatar.
        /// </summary>
        public virtual ICollection<ApplicationUser>? Users { get; set; }

        /// <summary>
        /// Navigation property to the collection of <see cref="Language"/> entities that use this file as their avatar (e.g., a flag image).
        /// </summary>
        public virtual ICollection<Language>? Languages { get; set; }
    }
}