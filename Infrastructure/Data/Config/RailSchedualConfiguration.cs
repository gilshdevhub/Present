using Core.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class RailSchedualConfiguration : IEntityTypeConfiguration<RailSchedual>
{
    public void Configure(EntityTypeBuilder<RailSchedual> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.CreateTime).IsRequired();
        _ = builder.Property(p => p.TrainNumber).IsRequired();
        _ = builder.Property(p => p.TrainDate).IsRequired();
        _ = builder.Property(p => p.StationOrder).IsRequired();
        _ = builder.Property(p => p.StationId).IsRequired();
        _ = builder.Property(p => p.StopCode).IsRequired();
        _ = builder.Property(p => p.Platform).IsRequired();
    }
}
