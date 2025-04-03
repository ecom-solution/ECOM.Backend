using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Persistence.Main.Configurations
{
	public class LanguageTranslationConfiguration : IEntityTypeConfiguration<LanguageTranslation>
	{
		public void Configure(EntityTypeBuilder<LanguageTranslation> builder)
		{
			builder.ToTable(nameof(LanguageTranslation));

			builder.HasKey(x => new { x.LanguageId, x.LanguageKeyId });

			builder.Property(x => x.Value).HasMaxLength(500).IsRequired();

			builder.HasOne(x => x.Language).WithMany(x => x.LanguageTranslations).HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Cascade);
			builder.HasOne(x => x.LanguageKey).WithMany(x => x.LanguageTranslations).HasForeignKey(x => x.LanguageKeyId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
