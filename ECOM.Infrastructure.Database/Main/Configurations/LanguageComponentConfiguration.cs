using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class LanguageComponentConfiguration : MainConfiguration, IEntityTypeConfiguration<LanguageComponent>
	{
		public void Configure(EntityTypeBuilder<LanguageComponent> builder)
		{
			builder.ToTable(nameof(LanguageComponent));

			builder.HasKey(x => x.Id);
			builder.HasIndex(x => x.ComponentName).IsUnique();

			builder.Property(x => x.ComponentName).HasMaxLength(255).IsUnicode(false).IsRequired();
			builder.Property(x => x.Description).HasMaxLength(500);

			builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId);
			builder.HasMany(x => x.LanguageKeys).WithOne(x => x.LanguageComponent).HasForeignKey(x => x.LanguageComponentId).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
