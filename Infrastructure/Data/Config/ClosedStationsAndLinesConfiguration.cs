using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Stations;

namespace Infrastructure.Data.Config;

public class ClosedStationsAndLinesConfiguration : IEntityTypeConfiguration<ClosedStationsAndLines>
{
    public void Configure(EntityTypeBuilder<ClosedStationsAndLines> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.ValidFrom);
        _ = builder.Property(p => p.ValidTo);
        _ = builder.Property(p => p.FromStation);
        _ = builder.Property(p => p.ToStation);
    }
}
