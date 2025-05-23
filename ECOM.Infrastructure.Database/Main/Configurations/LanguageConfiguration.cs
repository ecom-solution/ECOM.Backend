﻿using ECOM.Domain.Entities.Main;
using ECOM.Infrastructure.Database.Main.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class LanguageConfiguration : MainConfiguration, IEntityTypeConfiguration<Language>
	{
		public void Configure(EntityTypeBuilder<Language> builder)
		{
			builder.ToTable(nameof(Language));

			builder.HasKey(x => x.Id);
			builder.HasIndex(x => x.Code).IsUnique();
			builder.Property(x => x.Code).HasMaxLength(2).IsUnicode(false).IsRequired();
			builder.Property(x => x.Name).HasMaxLength(50).IsRequired();

			builder.HasMany(x => x.LanguageTranslations).WithOne(x => x.Language).HasForeignKey(x => x.LanguageId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
