using Microsoft.EntityFrameworkCore;
using ECOM.Domain.Entities.MainLogging;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECOM.Infrastructure.Database.MainLogging.Common;

namespace ECOM.Infrastructure.Database.MainLogging.Configurations
{
	public class TransactionLogConfiguration : MainLoggingConfiguration, IEntityTypeConfiguration<TransactionLog>
	{
		public void Configure(EntityTypeBuilder<TransactionLog> builder)
		{
			builder.ToTable(nameof(TransactionLog));

			builder.HasKey(x => x.Id);
			builder.Property(x => x.CreatedAt_Utc).IsRequired();
			builder.Property(x => x.Level).HasMaxLength(10).IsRequired();
			builder.Property(x => x.Message).HasMaxLength(int.MaxValue).IsRequired();
			builder.Property(x => x.Exception).HasMaxLength(int.MaxValue);
			builder.Property(x => x.Properties).HasMaxLength(int.MaxValue);
			builder.Property(x => x.CallerMethod).IsUnicode(false).HasMaxLength(1000);
			builder.Property(x => x.CallerFileName).IsUnicode(false).HasMaxLength(1000);
			builder.Property(x => x.IpAddress).IsUnicode(false).HasMaxLength(50);
			builder.Property(x => x.TransactionId).IsRequired();
		}
	}
}
