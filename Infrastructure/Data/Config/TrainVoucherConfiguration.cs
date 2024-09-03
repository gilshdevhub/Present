using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TrainVoucherConfiguration : IEntityTypeConfiguration<TrainVoucher>
{
    public void Configure(EntityTypeBuilder<TrainVoucher> builder)
    {
        _ = builder.Property(p => p.Id).ValueGeneratedOnAdd();
        _ = builder.HasKey(p => new { p.TrainId, p.TrainDate });
        _ = builder.Property(p => p.DepartureStationId).IsRequired();
        _ = builder.Property(p => p.DepartureTime).IsRequired();
        _ = builder.Property(p => p.ArrivalStationId).IsRequired();
        _ = builder.Property(p => p.ArrivalTime).IsRequired();
        _ = builder.Property(p => p.NOV).IsRequired();
        _ = builder.Property(p => p.CreationDate).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("getdate()");
        _ = builder.Property(p => p.LastUpdated).IsRequired().ValueGeneratedOnUpdate().HasDefaultValueSql("getdate()");
        _ = builder.HasMany(p => p.Items).WithOne(p => p.TrainVoucher).OnDelete(DeleteBehavior.Cascade);
    }
}
