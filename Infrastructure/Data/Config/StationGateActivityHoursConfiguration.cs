using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationGateActivityHoursConfiguration : IEntityTypeConfiguration<StationGateActivityHours>
{
    public void Configure(EntityTypeBuilder<StationGateActivityHours> builder)
    {
        _ = builder.HasKey(p => p.StationHoursId);
        _ = builder.Property(p => p.StationGateId).IsRequired();
        _ = builder.Property(p => p.TemplateTypeId).IsRequired();
        _ = builder.Property(p => p.IsClosed);
        _ = builder.Property(p => p.ClosedUntill);
    }
}
