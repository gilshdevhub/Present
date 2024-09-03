using Core.Entities.Push;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

class PushNotificationsByDateConfiguration : IEntityTypeConfiguration<PushNotificationsByDate>
{
    public void Configure(EntityTypeBuilder<PushNotificationsByDate> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("[dbo].[dReturnDate](getdate())");
        _ = builder.Property(p => p.TrainNumber).IsRequired();
        _ = builder.Property(p => p.DepartureTime).IsRequired();
        _ = builder.Property(p => p.ArrivalTime).IsRequired();
        _ = builder.Property(p => p.DepartureStationId).IsRequired();
        _ = builder.Property(p => p.ArrivalStationId).IsRequired();
        _ = builder.Property(p => p.DepartutePlatform).IsRequired();
        _ = builder.Property(p => p.ArrivalPlatform).IsRequired();
        _ = builder.Property(p => p.TrainDate).IsRequired();
        _ = builder.Property(p => p.PushRoutingId).IsRequired();
    }
}
