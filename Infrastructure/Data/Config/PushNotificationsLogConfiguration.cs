using Core.Entities.Push;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class PushNotificationsLogConfiguration : IEntityTypeConfiguration<PushNotificationsLog>
{
    public void Configure(EntityTypeBuilder<PushNotificationsLog> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.PushRegistrationId).IsRequired();
        _ = builder.Property(p => p.PushRoutingId).IsRequired();
        _ = builder.Property(p => p.CreatedDate).IsRequired();
        _ = builder.Property(p => p.TrainNumber).IsRequired();
        _ = builder.Property(p => p.DepartureTime).IsRequired();
        _ = builder.Property(p => p.ArrivalTime).IsRequired();
        _ = builder.Property(p => p.DepartureStationId).IsRequired();
        _ = builder.Property(p => p.ArrivalStationId).IsRequired();
        _ = builder.Property(p => p.DepartutePlatform).IsRequired();
        _ = builder.Property(p => p.ArrivalPlatform).IsRequired();
    }
}
