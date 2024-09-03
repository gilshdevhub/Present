using Core.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class NotificationTypeConfiguration : IEntityTypeConfiguration<NotificationType>
{
    public void Configure(EntityTypeBuilder<NotificationType> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Id).ValueGeneratedNever();
        _ = builder.Property(p => p.Description).IsRequired().HasMaxLength(1000);

        _ = builder.HasMany(p => p.AutomationNotifications).WithOne(p => p.NotificationType).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.NotificationEvents).WithOne(p => p.NotificationType).OnDelete(DeleteBehavior.NoAction);
    }
}
