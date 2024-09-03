using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationServicesConfiguration : IEntityTypeConfiguration<StationServices>
{
    public void Configure(EntityTypeBuilder<StationServices> builder)
    {
        _ = builder.HasKey(p => p.ServiceId);
        _ = builder.Property(p => p.Hebrew).HasMaxLength(50).IsRequired();
        _ = builder.Property(p => p.English).HasMaxLength(50);
        _ = builder.Property(p => p.Arabic).HasMaxLength(50);
        _ = builder.Property(p => p.Russian).HasMaxLength(50);
        _ = builder.Property(p => p.IconId).HasMaxLength(50).IsRequired();
        _ = builder.Property(p => p.IconLink).HasMaxLength(512).IsRequired();
    }
}
