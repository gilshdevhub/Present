using Core.Entities.Push;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

class PushNotificationsByWeekDayConfiguration : IEntityTypeConfiguration<PushNotificationsByWeekDay>
{
    public void Configure(EntityTypeBuilder<PushNotificationsByWeekDay> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("[dbo].[dReturnDate](getdate())");
        _ = builder.Property(p => p.TrainNumber).IsRequired();
        _ = builder.Property(p => p.DepartureStationId).IsRequired();
        _ = builder.Property(p => p.ArrivalStationId).IsRequired();
        _ = builder.Property(p => p.DepartureTime).IsRequired();
        _ = builder.Property(p => p.ArrivalTime).IsRequired();
        _ = builder.Property(p => p.DepartutePlatform).IsRequired();
        _ = builder.Property(p => p.ArrivalPlatform).IsRequired();
        _ = builder.Property(p => p.day1).IsRequired();
        _ = builder.Property(p => p.day2).IsRequired();
        _ = builder.Property(p => p.day3).IsRequired();
        _ = builder.Property(p => p.day4).IsRequired();
        _ = builder.Property(p => p.day5).IsRequired();
        _ = builder.Property(p => p.day6).IsRequired();
        _ = builder.Property(p => p.day7).IsRequired();


    }
}
