using Core.Entities.Push;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class PushRoutingWeekSchedualConfiguration : IEntityTypeConfiguration<PushRoutingWeekSchedual>
{
    public void Configure(EntityTypeBuilder<PushRoutingWeekSchedual> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.WeekDay).IsRequired();
        _ = builder.Property(p => p.PushRoutingId).IsRequired();
    }
}
