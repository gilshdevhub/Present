using Core.Entities.MotUpdates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class MotUpdatesConfiguration : IEntityTypeConfiguration<MotConvertion>
{
    public void Configure(EntityTypeBuilder<MotConvertion> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.StationId).IsRequired();
        _ = builder.Property(p => p.MonitoringRef).IsRequired();
    }
}
