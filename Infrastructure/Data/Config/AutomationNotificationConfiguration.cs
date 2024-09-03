using Core.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class AutomationNotificationConfiguration : IEntityTypeConfiguration<AutomationNotification>
{
    public void Configure(EntityTypeBuilder<AutomationNotification> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.CreateDate).IsRequired();
        _ = builder.Property(p => p.TrainNumber).IsRequired();
        _ = builder.Property(p => p.TrainDate).IsRequired();
        _ = builder.Property(p => p.NotificationTypeId).IsRequired();
    }
}
