using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class LanguageKeyConfiguration : MainConfiguration, IEntityTypeConfiguration<LanguageKey>
	{
		public void Configure(EntityTypeBuilder<LanguageKey> builder)
		{
			builder.ToTable(nameof(LanguageKey));

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Key).HasMaxLength(500).IsRequired();
			builder.Property(x => x.Description).HasMaxLength(500);

			builder.HasOne(x => x.LanguageComponent).WithMany(x => x.LanguageKeys).HasForeignKey(x => x.LanguageComponentId).OnDelete(DeleteBehavior.Cascade);
			builder.HasMany(x => x.LanguageTranslations).WithOne(x => x.LanguageKey).HasForeignKey(x => x.LanguageKeyId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
