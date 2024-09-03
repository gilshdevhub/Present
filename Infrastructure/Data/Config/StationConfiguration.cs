using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationConfiguration : IEntityTypeConfiguration<Station>
{
    public void Configure(EntityTypeBuilder<Station> builder)
    {
        _ = builder.HasKey(p => p.StationId);
        _ = builder.Property(p => p.StationId).ValueGeneratedNever();
        _ = builder.Property(p => p.HebrewName).IsRequired().HasMaxLength(100).IsUnicode();
        _ = builder.Property(p => p.RjpaName).IsRequired().HasMaxLength(100).IsUnicode();
        _ = builder.Property(p => p.EnglishName).IsRequired(false).HasMaxLength(100).IsUnicode();
        _ = builder.Property(p => p.RussianName).IsRequired(false).HasMaxLength(100).IsUnicode();
        _ = builder.Property(p => p.ArabicName).IsRequired(false).HasMaxLength(100).IsUnicode();
        _ = builder.Property(p => p.Lontitude).HasPrecision(18, 11);
        _ = builder.Property(p => p.Latitude).HasPrecision(18, 11);
        _ = builder.Property(p => p.TicketStationId);
        _ = builder.HasMany(p => p.ArrivalTrainTimeTable).WithOne(p => p.ArrivalStation).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.DepartureTrainTimeTable).WithOne(p => p.DepartureStation).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.TrainStationsTimeTable).WithOne(p => p.Station).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.ArrivalDailyTrainTimeTable).WithOne(p => p.ArrivalStation).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.DepartureDailyTrainTimeTable).WithOne(p => p.DepartureStation).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.DailyTrainStationsTimeTable).WithOne(p => p.Station).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.ArrivalTrainVoucher).WithOne(p => p.ArrivalStation).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.DepartureTrainVoucher).WithOne(p => p.DepartureStation).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.TrainStationVoucher).WithOne(p => p.Station).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.Synonym).WithOne(p => p.Station).OnDelete(DeleteBehavior.Cascade);
        _ = builder.HasMany(p => p.ChangedStationAutomationNotification).WithOne(p => p.ChangedStation).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.CompensationOriginStation).WithOne(p => p.OriginStation).OnDelete(DeleteBehavior.NoAction);
        _ = builder.HasMany(p => p.CompensationDestinationStation).WithOne(p => p.DestinationStation).OnDelete(DeleteBehavior.NoAction);
    }
}
