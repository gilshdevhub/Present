using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationInfoTranslationConfiguration : IEntityTypeConfiguration<StationInfoTranslation>
{
    public void Configure(EntityTypeBuilder<StationInfoTranslation> builder)
    {
        _ = builder.HasKey(nameof(StationInfoTranslation.Key), nameof(StationInfoTranslation.StationId));
        _ = builder.Property(p => p.Hebrew).HasMaxLength(10000).IsRequired();
        _ = builder.Property(p => p.English).HasMaxLength(10000);
        _ = builder.Property(p => p.Arabic).HasMaxLength(10000);
        _ = builder.Property(p => p.Russian).HasMaxLength(10000);
        _ = builder.Property(p => p.Description).HasMaxLength(512);
    }
}
