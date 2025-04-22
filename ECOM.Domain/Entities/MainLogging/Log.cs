namespace ECOM.Domain.Entities.MainLogging
{
    /// <summary>
    /// Represents a log entry in the main logging system.
    /// This entity stores detailed information about events that occur within the application,
    /// including timestamps, severity levels, messages, and contextual data.
    /// </summary>
    public class Log : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log() { }

        /// <summary>
        /// Gets or sets the UTC timestamp when the log entry was created.
        /// This property is required and provides a precise record of when the event occurred.
        /// </summary>
        public DateTime CreatedAt_Utc { get; set; }

        /// <summary>
        /// Gets or sets the severity level of the log entry (e.g., "Information", "Warning", "Error").
        /// This property is required and helps categorize the importance of the log event.
        /// Defaults to an empty string.
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the main message of the log entry, describing the event that occurred.
        /// This property is required and provides the primary textual information of the log.
        /// Defaults to an empty string.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the details of any exception that occurred during the event, if applicable.
        /// This property is nullable and can contain stack traces and other exception information.
        /// </summary>
        public string? Exception { get; set; }

        /// <summary>
        /// Gets or sets additional properties or structured data associated with the log entry, often stored as JSON.
        /// This property is nullable and allows for including contextual information beyond the main message.
        /// </summary>
        public string? Properties { get; set; }

        /// <summary>
        /// Gets or sets the name of the method where the log entry originated.
        /// This property is nullable and helps in tracing the source of the log event.
        /// </summary>
        public string? CallerMethod { get; set; }

        /// <summary>
        /// Gets or sets the name of the file where the log entry originated.
        /// This property is nullable and further aids in pinpointing the log's source.
        /// </summary>
        public string? CallerFileName { get; set; }

        /// <summary>
        /// Gets or sets the line number in the source file where the log entry originated.
        /// This integer property is nullable and provides the most granular level of source tracking.
        /// </summary>
        public int? CallerLineNumber { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the client or server associated with the log event, if relevant.
        /// This property is nullable and can be useful for tracking user activity or system interactions.
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user associated with the log event, if applicable.
        /// This GUID property is nullable and allows for linking log entries to specific users.
        /// </summary>
        public Guid? UserId { get; set; }
    }
}