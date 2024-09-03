using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class DailyTrainsTimeTableConfiguration : IEntityTypeConfiguration<DailyTrainsTimeTable>
{
    public void Configure(EntityTypeBuilder<DailyTrainsTimeTable> builder)
    {
        _ = builder.HasKey(p => p.TrainId);
        _ = builder.Property(p => p.TrainId).ValueGeneratedNever();
        _ = builder.Property(p => p.TrainDate).IsRequired();
        _ = builder.Property(p => p.DepartureStationId).IsRequired();
        _ = builder.Property(p => p.DepartureTime).IsRequired();
        _ = builder.Property(p => p.ArrivalStationId).IsRequired();
        _ = builder.Property(p => p.ArrivalTime).IsRequired();
        _ = builder.Property(p => p.ChangeType).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.CreationDate).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("getdate()");
        _ = builder.Property(p => p.LastUpdated).IsRequired().ValueGeneratedOnUpdate().HasDefaultValueSql("getdate()");

        _ = builder.HasMany(p => p.Items).WithOne(p => p.DailyTrainsTimeTable).OnDelete(DeleteBehavior.Cascade);
    }
}
