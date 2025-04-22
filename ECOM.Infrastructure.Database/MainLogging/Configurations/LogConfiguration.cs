using Microsoft.EntityFrameworkCore;
using ECOM.Domain.Entities.MainLogging;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECOM.Infrastructure.Database.MainLogging.Common;

namespace ECOM.Infrastructure.Database.MainLogging.Configurations
{
    /// <summary>
    /// Configuration for the <see cref="Log"/> entity using Fluent API.
    /// This class defines how the <see cref="Log"/> entity maps to the 'Log' table
    /// in the MainLogging database schema.
    /// </summary>
    public class LogConfiguration : MainLoggingConfiguration, IEntityTypeConfiguration<Log>
    {
        /// <summary>
        /// Configures the <see cref="Log"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            // Maps the entity to the 'Log' table in the database.
            builder.ToTable(nameof(Log));

            // Configures the primary key for the Log entity, which is the 'Id' property.
            builder.HasKey(x => x.Id);

            // Configures the 'CreatedAt_Utc' property:
            // - It is required (cannot be null).
            builder.Property(x => x.CreatedAt_Utc).IsRequired();

            // Configures the 'Level' property:
            // - It has a maximum length of 10 characters.
            // - It is required (cannot be null).
            builder.Property(x => x.Level).HasMaxLength(10).IsRequired();

            // Configures the 'Message' property:
            // - It has a maximum length of the maximum integer value, allowing for long log messages.
            // - It is required (cannot be null).
            builder.Property(x => x.Message).HasMaxLength(int.MaxValue).IsRequired();

            // Configures the 'Exception' property:
            // - It has a maximum length of the maximum integer value, allowing for detailed exception information.
            // - It is nullable by default (not explicitly set as required).
            builder.Property(x => x.Exception).HasMaxLength(int.MaxValue);

            // Configures the 'Properties' property:
            // - It has a maximum length of the maximum integer value, allowing for storing various log properties as JSON or other formats.
            // - It is nullable by default.
            builder.Property(x => x.Properties).HasMaxLength(int.MaxValue);

            // Configures the 'CallerMethod' property:
            // - It is not Unicode, allowing for efficient storage of ASCII-based method names.
            // - It has a maximum length of 1000 characters.
            // - It is nullable by default.
            builder.Property(x => x.CallerMethod).IsUnicode(false).HasMaxLength(1000);

            // Configures the 'CallerFileName' property:
            // - It is not Unicode.
            // - It has a maximum length of 1000 characters.
            // - It is nullable by default.
            builder.Property(x => x.CallerFileName).IsUnicode(false).HasMaxLength(1000);

            // Configures the 'IpAddress' property:
            // - It is not Unicode.
            // - It has a maximum length of 50 characters, suitable for storing IPv4 or IPv6 addresses.
            // - It is nullable by default.
            builder.Property(x => x.IpAddress).IsUnicode(false).HasMaxLength(50);
        }
    }
}