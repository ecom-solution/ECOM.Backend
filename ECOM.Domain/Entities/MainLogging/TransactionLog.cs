namespace ECOM.Domain.Entities.MainLogging
{
    /// <summary>
    /// Represents a log entry specifically for tracking transactions within the application.
    /// This entity inherits common logging properties from <see cref="BaseEntity"/> and includes
    /// a unique identifier for the transaction being logged.
    /// </summary>
    public class TransactionLog : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionLog"/> class.
        /// </summary>
        public TransactionLog() { }

        /// <summary>
        /// Gets or sets the UTC timestamp when the transaction log entry was created.
        /// This property is required and indicates the time of the logging event.
        /// </summary>
        public DateTime CreatedAt_Utc { get; set; }

        /// <summary>
        /// Gets or sets the severity level of the transaction log entry (e.g., "Information", "Debug", "Error").
        /// This property is required and categorizes the importance of the logged transaction event.
        /// Defaults to an empty string.
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the descriptive message of the transaction log entry.
        /// This property is required and provides the main information about the logged transaction step or event.
        /// Defaults to an empty string.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets details of any exception that occurred during the transaction, if any.
        /// This property is nullable and can contain stack traces and exception messages.
        /// </summary>
        public string? Exception { get; set; }

        /// <summary>
        /// Gets or sets additional properties or structured data related to the transaction, often stored as JSON.
        /// This property is nullable and allows for including contextual details about the transaction.
        /// </summary>
        public string? Properties { get; set; }

        /// <summary>
        /// Gets or sets the name of the method where the transaction log entry originated.
        /// This property is nullable and helps in identifying the code that logged the transaction event.
        /// </summary>
        public string? CallerMethod { get; set; }

        /// <summary>
        /// Gets or sets the name of the file where the transaction log entry originated.
        /// This property is nullable and further helps in locating the source of the log.
        /// </summary>
        public string? CallerFileName { get; set; }

        /// <summary>
        /// Gets or sets the line number in the source file where the transaction log entry was created.
        /// This integer property is nullable and provides a precise location of the log statement.
        /// </summary>
        public int? CallerLineNumber { get; set; }

        /// <summary>
        /// Gets or sets the IP address associated with the transaction, if applicable.
        /// This property is nullable and can be useful for tracking transactions initiated by specific clients or servers.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user involved in the transaction, if any.
        /// This GUID property is nullable and allows for associating transaction logs with specific users.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the transaction being logged.
        /// This property is required and serves as the key to group related log entries for a single transaction.
        /// </summary>
        public Guid TransactionId { get; set; }
    }
}