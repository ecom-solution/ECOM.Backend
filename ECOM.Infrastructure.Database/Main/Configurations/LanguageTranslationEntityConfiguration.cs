using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class LanguageTranslationEntityConfiguration : MainConfiguration, IEntityTypeConfiguration<LanguageTranslationEntity>
	{
		public void Configure(EntityTypeBuilder<LanguageTranslationEntity> builder)
		{
			builder.ToTable(nameof(LanguageTranslationEntity));

			builder.HasKey(x => new { x.LanguageId, x.EntityName, x.EntityId, x.FieldName });

			builder.Property(x => x.EntityName).HasMaxLength(100).IsRequired();
			builder.Property(x => x.FieldName).HasMaxLength(100).IsRequired();
			builder.Property(x => x.Value).HasMaxLength(5000).IsRequired();

			builder.HasOne(x => x.Language).WithMany(x => x.LanguageTranslationEntities).HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
