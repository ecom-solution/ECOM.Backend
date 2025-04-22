using Microsoft.EntityFrameworkCore;
using ECOM.Domain.Entities.MainLogging;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECOM.Infrastructure.Database.MainLogging.Common;

namespace ECOM.Infrastructure.Database.MainLogging.Configurations
{
    /// <summary>
    /// Configuration for the <see cref="TransactionLog"/> entity using Fluent API.
    /// This class defines how the <see cref="TransactionLog"/> entity maps to the
    /// 'TransactionLog' table in the MainLogging database schema. It inherits
    /// common configurations from <see cref="MainLoggingConfiguration"/>.
    /// </summary>
    public class TransactionLogConfiguration : MainLoggingConfiguration, IEntityTypeConfiguration<TransactionLog>
    {
        /// <summary>
        /// Configures the <see cref="TransactionLog"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<TransactionLog> builder)
        {
            // Maps the entity to the 'TransactionLog' table in the database.
            builder.ToTable(nameof(TransactionLog));

            // Configures the primary key for the TransactionLog entity, which is the 'Id' property.
            builder.HasKey(x => x.Id);

            // Configures the 'CreatedAt_Utc' property:
            // - It is required (cannot be null), indicating the timestamp of the log entry.
            builder.Property(x => x.CreatedAt_Utc).IsRequired();

            // Configures the 'Level' property:
            // - It has a maximum length of 10 characters, representing the severity level of the log.
            // - It is required (cannot be null).
            builder.Property(x => x.Level).HasMaxLength(10).IsRequired();

            // Configures the 'Message' property:
            // - It has a maximum length of the maximum integer value, allowing for detailed log messages.
            // - It is required (cannot be null), containing the core information of the log event.
            builder.Property(x => x.Message).HasMaxLength(int.MaxValue).IsRequired();

            // Configures the 'Exception' property:
            // - It has a maximum length of the maximum integer value, allowing for storing detailed exception information if an error occurred.
            // - It is nullable by default (not explicitly set as required), as not all transaction logs involve exceptions.
            builder.Property(x => x.Exception).HasMaxLength(int.MaxValue);

            // Configures the 'Properties' property:
            // - It has a maximum length of the maximum integer value, allowing for storing structured log data or additional properties as JSON or other formats.
            // - It is nullable by default.
            builder.Property(x => x.Properties).HasMaxLength(int.MaxValue);

            // Configures the 'CallerMethod' property:
            // - It is not Unicode, optimizing storage for ASCII-based method names.
            // - It has a maximum length of 1000 characters, indicating the method that initiated the log.
            // - It is nullable by default.
            builder.Property(x => x.CallerMethod).IsUnicode(false).HasMaxLength(1000);

            // Configures the 'CallerFileName' property:
            // - It is not Unicode.
            // - It has a maximum length of 1000 characters, indicating the file where the log originated.
            // - It is nullable by default.
            builder.Property(x => x.CallerFileName).IsUnicode(false).HasMaxLength(1000);

            // Configures the 'IpAddress' property:
            // - It is not Unicode.
            // - It has a maximum length of 50 characters, suitable for storing IPv4 or IPv6 addresses associated with the transaction.
            // - It is nullable by default.
            builder.Property(x => x.IpAddress).IsUnicode(false).HasMaxLength(50);

            // Configures the 'TransactionId' property:
            // - It is required (cannot be null), representing a unique identifier for the transaction being logged.
            builder.Property(x => x.TransactionId).IsRequired();
        }
    }
}