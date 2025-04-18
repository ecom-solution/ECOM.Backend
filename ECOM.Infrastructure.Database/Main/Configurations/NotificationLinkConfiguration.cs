using ECOM.Domain.Entities.Main;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECOM.Infrastructure.Database.Main.Configurations
{
	public class NotificationLinkConfiguration : IEntityTypeConfiguration<NotificationLink>
	{
		public void Configure(EntityTypeBuilder<NotificationLink> builder)
		{
			builder.ToTable(nameof(NotificationLink));

			builder.HasKey(x => x.Id);

			builder.Property(x => x.Label)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(x => x.Url)
				.HasMaxLength(1000)
				.IsRequired();

			builder.Property(x => x.OrderIndex)
				.IsRequired();

			builder.Property(x => x.NotificationId)
				.IsRequired();

			builder.HasOne(x => x.Notification)
				.WithMany(x => x.NotificationLinks)
				.HasForeignKey(x => x.NotificationId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
