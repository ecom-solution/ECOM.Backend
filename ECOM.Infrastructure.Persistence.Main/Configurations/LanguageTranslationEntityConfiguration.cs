using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Persistence.Main.Configurations
{
	public class LanguageTranslationEntityConfiguration : IEntityTypeConfiguration<LanguageTranslationEntity>
	{
		public void Configure(EntityTypeBuilder<LanguageTranslationEntity> builder)
		{
			builder.ToTable(nameof(LanguageTranslationEntity));

			builder.HasKey(x => new { x.LanguageId, x.EntityName, x.EntityId, x.FieldName });

			builder.Property(x => x.EntityName).HasMaxLength(100).IsRequired();
			builder.Property(x => x.FieldName).HasMaxLength(100).IsRequired();
			builder.Property(x => x.Value).HasMaxLength(5000).IsRequired();
		}
	}
}
