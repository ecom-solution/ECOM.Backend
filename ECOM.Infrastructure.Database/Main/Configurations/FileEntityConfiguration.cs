using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
    /// <summary>
    /// Configuration class for the <see cref="FileEntity"/> entity, defining its mapping to the database table,
    /// properties related to file storage, and its relationships with other entities.
    /// This class implements the <see cref="IEntityTypeConfiguration{FileEntity}"/> interface.
    /// </summary>
    public class FileEntityConfiguration : MainConfiguration, IEntityTypeConfiguration<FileEntity>
    {
        /// <summary>
        /// Configures the <see cref="FileEntity"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.ToTable(nameof(FileEntity)); // Maps the entity to a table named "FileEntity"

            builder.HasKey(x => x.Id); // Defines the "Id" property as the primary key

            builder.Property(x => x.FileName).HasMaxLength(255).IsRequired(); // Configures the "FileName" property: maximum length 255 and required
            builder.Property(x => x.FileUrl).HasMaxLength(500).IsRequired(); // Configures the "FileUrl" property: maximum length 500 and required
            builder.Property(x => x.FileSize).IsRequired(); // Configures the "FileSize" property as required
            builder.Property(x => x.ContentType).HasMaxLength(100).IsRequired(); // Configures the "ContentType" property: maximum length 100 and required
            builder.Property(x => x.BucketName).HasMaxLength(100).IsRequired(); // Configures the "BucketName" property: maximum length 100 and required

            // Configures the one-to-many relationship between FileEntity (as Avatar) and ApplicationUser
            builder.HasMany(x => x.Users) // FileEntity can be the Avatar for many ApplicationUsers
                .WithOne(x => x.Avartar) // Each ApplicationUser has one Avatar (nullable)
                .HasForeignKey(x => x.AvatarId) // Defines the foreign key "AvatarId" in the ApplicationUser table
                .OnDelete(DeleteBehavior.SetNull); // Configures the delete behavior: when a FileEntity (Avatar) is deleted, the AvatarId of associated users is set to null

            // Configures the one-to-many relationship between FileEntity (as Avatar) and Language
            builder.HasMany(x => x.Languages) // FileEntity can be the Avatar for many Languages
                .WithOne(x => x.Avatar) // Each Language has one Avatar (nullable)
                .HasForeignKey(x => x.AvatarId) // Defines the foreign key "AvatarId" in the Language table
                .OnDelete(DeleteBehavior.SetNull); // Configures the delete behavior: when a FileEntity (Avatar) is deleted, the AvatarId of associated languages is set to null
        }
    }
}