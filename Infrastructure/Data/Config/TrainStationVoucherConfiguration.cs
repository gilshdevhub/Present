using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TrainStationVoucherConfiguration : IEntityTypeConfiguration<TrainStationVoucher>
{
    public void Configure(EntityTypeBuilder<TrainStationVoucher> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.StationSequence).IsRequired();
        _ = builder.Property(p => p.StationId).IsRequired();
        _ = builder.Property(p => p.DepartureTime).IsRequired();
        _ = builder.Property(p => p.ArrivalTime).IsRequired();
        _ = builder.Property(p => p.NOV).IsRequired();
        _ = builder.Property(p => p.CreationDate).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("getdate()");
        _ = builder.Property(p => p.LastUpdated).IsRequired().ValueGeneratedOnUpdate().HasDefaultValueSql("getdate()");
    }
}
