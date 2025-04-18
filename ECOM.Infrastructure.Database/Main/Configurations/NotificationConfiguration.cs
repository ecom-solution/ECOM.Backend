using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
	{
		public void Configure(EntityTypeBuilder<Notification> builder)
		{
			builder.ToTable(nameof(Notification));

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Title)
				.HasMaxLength(200)
				.IsRequired();

			builder.Property(x => x.Content)
				.IsRequired();

			builder.Property(x => x.Type)
				.IsRequired();

			builder.HasMany(x => x.NotificationLinks)
				.WithOne(x => x.Notification)
				.HasForeignKey(x => x.NotificationId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
