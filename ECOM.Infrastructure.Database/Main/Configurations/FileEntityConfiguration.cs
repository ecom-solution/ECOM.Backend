using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class FileEntityConfiguration : MainConfiguration, IEntityTypeConfiguration<FileEntity>
	{
		public void Configure(EntityTypeBuilder<FileEntity> builder)
		{
			builder.ToTable(nameof(FileEntity));

			builder.HasKey(x => x.Id);

			builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
			builder.Property(x => x.FileUrl).HasMaxLength(500).IsRequired();
			builder.Property(x => x.FileSize).IsRequired();
			builder.Property(x => x.ContentType).HasMaxLength(100).IsRequired();
			builder.Property(x => x.BucketName).HasMaxLength(100).IsRequired();

			builder.HasMany(x => x.Users).WithOne(x => x.Avartar).HasForeignKey(x => x.AvatarId).OnDelete(DeleteBehavior.SetNull);
			builder.HasMany(x => x.Languages).WithOne(x => x.Avatar).HasForeignKey(x => x.AvatarId).OnDelete(DeleteBehavior.SetNull);
		}
	}
}
