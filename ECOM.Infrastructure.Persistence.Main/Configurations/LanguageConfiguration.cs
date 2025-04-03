using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Persistence.Main.Configurations
{
	public class LanguageConfiguration : IEntityTypeConfiguration<Language>
	{
		public void Configure(EntityTypeBuilder<Language> builder)
		{
			builder.ToTable(nameof(Language));

			builder.HasKey(x => x.Id);
			builder.Property(x => x.Code).HasMaxLength(5).IsUnicode(false).IsRequired();
			builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

			builder.HasMany(x => x.LanguageTranslations).WithOne(x => x.Language).HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
