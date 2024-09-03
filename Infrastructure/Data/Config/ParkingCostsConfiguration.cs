using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ParkingCostsConfiguration : IEntityTypeConfiguration<ParkingCosts>
{
    public void Configure(EntityTypeBuilder<ParkingCosts> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Value).HasMaxLength(50).IsRequired();
        _ = builder.Property(p => p.Key).HasMaxLength(50).IsRequired();
    }
}
