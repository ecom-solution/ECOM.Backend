using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration for the <see cref="SeedState"/> entity using Fluent API.
    /// </summary>
    public class SeedStateConfiguration : IEntityTypeConfiguration<SeedState>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<SeedState> builder)
        {
            builder.ToTable(nameof(SeedState));

            // Primary Key
            builder.HasKey(ss => ss.Id);

            // Properties
            builder.Property(ss => ss.SeedName)
                .HasMaxLength(256)
                .IsRequired();
            builder.HasIndex(ss => ss.SeedName)
                .IsUnique(); // Ensure only one state record per seed name

            builder.Property(ss => ss.CurrentHash)
                .HasMaxLength(512); // Length of an MD5 hash is 32 hex chars, SHA256 is 64, so 512 should be safe

            builder.Property(ss => ss.LastSeededAtUtc)
                .IsRequired();

            builder.Property(ss => ss.LastModifiedAtUtc)
                .IsRequired(false);
        }
    }
}