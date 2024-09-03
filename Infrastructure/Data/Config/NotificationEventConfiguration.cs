using Core.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class NotificationEventConfiguration : IEntityTypeConfiguration<NotificationEvent>
{
    public void Configure(EntityTypeBuilder<NotificationEvent> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.CreateDate).ValueGeneratedOnAdd().HasDefaultValueSql("[dbo].[dReturnDate](getdate())");
        _ = builder.Property(p => p.PushNotificationId).IsRequired();
        _ = builder.Property(p => p.PushRegistrationId).IsRequired();
        _ = builder.Property(p => p.AutomationNotificationId).IsRequired();
        _ = builder.Property(p => p.Message).IsRequired().HasMaxLength(5000);
        _ = builder.Property(p => p.TimeToSend).IsRequired();
        _ = builder.Property(p => p.NotificationTypeId).IsRequired();
        _ = builder.Property(p => p.Status).IsRequired();
    }
}
