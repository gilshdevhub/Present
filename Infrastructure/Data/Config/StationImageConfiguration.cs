using Core.Entities.Stations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

internal class StationImageConfiguration : IEntityTypeConfiguration<StationImage>
{
    public void Configure(EntityTypeBuilder<StationImage> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.ElementKey).IsRequired().HasMaxLength(20);
        _ = builder.Property(p => p.ElementName).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.ElementCode).IsRequired();
        _ = builder.Property(p => p.StationmInit).IsRequired(false);
        _ = builder.Property(p => p.IsInactive).IsRequired().HasDefaultValue(false);
        _ = builder.Property(p => p.FromDate).IsRequired();
        _ = builder.Property(p => p.ToDate).IsRequired(false);
        _ = builder.Property(p => p.LastUpdated).IsRequired(false);
        _ = builder.Property(p => p.BetweenDatesInactive).IsRequired().HasDefaultValue(false);
        _ = builder.Property(p => p.ElementCodeHE).IsRequired(false);
        _ = builder.Property(p => p.ElementCodeEN).IsRequired(false);
        _ = builder.Property(p => p.ElementCodeRU).IsRequired(false);
        _ = builder.Property(p => p.ElementCodeAR).IsRequired(false);
     }
}
