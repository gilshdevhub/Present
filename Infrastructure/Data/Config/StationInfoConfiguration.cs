using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationInfoConfiguration : IEntityTypeConfiguration<StationInfo>
{
    public void Configure(EntityTypeBuilder<StationInfo> builder)
    {
        _ = builder.HasKey(p => p.StationInfoId);
        _ = builder.Property(p => p.StationId).IsRequired();
        _ = builder.Property(p => p.LinesMapsX).IsRequired();
        _ = builder.Property(p => p.LinesMapsY).IsRequired();
        _ = builder.Property(p => p.ParkingCosts).IsRequired();
        _ = builder.Property(p => p.BikeParking).IsRequired();
        _ = builder.Property(p => p.BikeParkingCosts).IsRequired();
        _ = builder.Property(p => p.AirPolution);
        _ = builder.Property(p => p.StationMap).HasMaxLength(512);
        _ = builder.Property(p => p.NonActiveElavators).HasMaxLength(50);
        _ = builder.Property(p => p.StationIsClosed);
        _ = builder.Property(p => p.StationIsClosedUntill);
        _ = builder.Property(p => p.StationInfoFromDate);
        _ = builder.Property(p => p.StationInfoToDate);
    }
}
