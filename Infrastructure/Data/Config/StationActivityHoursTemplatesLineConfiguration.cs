using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationActivityHoursTemplatesLineConfiguration : IEntityTypeConfiguration<StationActivityHoursTemplatesLine>
{
    public void Configure(EntityTypeBuilder<StationActivityHoursTemplatesLine> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.TemplateId).IsRequired();
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
