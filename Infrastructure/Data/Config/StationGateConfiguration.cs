using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationGateConfiguration : IEntityTypeConfiguration<StationGate>
{
    public void Configure(EntityTypeBuilder<StationGate> builder)
    {
        _ = builder.HasKey(p => p.StationGateId);
        _ = builder.Property(p => p.StationId).IsRequired();
        _ = builder.Property(p => p.GateParking);
        _ = builder.Property(p => p.GateNameTranslationKey).HasMaxLength(50).IsRequired();
        _ = builder.Property(p => p.GateAddressTranslationKey).HasMaxLength(50);
        _ = builder.Property(p => p.GateLatitude).HasPrecision(18, 11).IsRequired();
        _ = builder.Property(p => p.GateLontitude).HasPrecision(18, 11).IsRequired();
        _ = builder.Property(p => p.GateOrder).IsRequired();
        _ = builder.Property(p => p.GateClosed);
        _ = builder.Property(p => p.GateClosedUntill);
    }
}
