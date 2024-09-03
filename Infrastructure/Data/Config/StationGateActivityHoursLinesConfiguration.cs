using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationGateActivityHoursLinesConfiguration : IEntityTypeConfiguration<StationGateActivityHoursLines>
{
    public void Configure(EntityTypeBuilder<StationGateActivityHoursLines> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.StationHoursId).IsRequired();
        _ = builder.Property(p => p.ActivityDaysNumbers).HasMaxLength(50);
        _ = builder.Property(p => p.StartHourPrefixTextKey).HasMaxLength(50);
        _ = builder.Property(p => p.StartHour).HasMaxLength(50);
        _ = builder.Property(p => p.StartHourReplaceTextKey).HasMaxLength(50);
        _ = builder.Property(p => p.EndHourPrefixTextKey).HasMaxLength(50);
        _ = builder.Property(p => p.EndHour).HasMaxLength(50);
        _ = builder.Property(p => p.EndHourReplaceTextKey).HasMaxLength(50);
        _ = builder.Property(p => p.EndHourPostfixTextKey).HasMaxLength(50);
        _ = builder.Property(p => p.ActivityHoursReplaceTextKey).HasMaxLength(50);
    }
}
