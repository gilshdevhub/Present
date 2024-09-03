using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TrainTimeTableConfiguration : IEntityTypeConfiguration<TrainTimeTable>
{
    public void Configure(EntityTypeBuilder<TrainTimeTable> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.TrainId).IsRequired();
        _ = builder.Property(p => p.TrainAlias).IsRequired(false).HasMaxLength(30);
        _ = builder.Property(p => p.DepartureStationId).IsRequired();
        _ = builder.Property(p => p.DepartureTime).IsRequired();
        _ = builder.Property(p => p.ArrivalStationId).IsRequired();
        _ = builder.Property(p => p.ArrivalTime).IsRequired();
        _ = builder.Property(p => p.TimeTableStartDate).IsRequired();
        _ = builder.Property(p => p.TimeTableEndDate).IsRequired();
        _ = builder.Property(p => p.CreationDate).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("getdate()");
        _ = builder.Property(p => p.LastUpdated).IsRequired().ValueGeneratedOnUpdate().HasDefaultValueSql("getdate()");
        _ = builder.HasMany(p => p.Items).WithOne(p => p.TrainTimeTable).OnDelete(DeleteBehavior.Cascade);
    }
}
