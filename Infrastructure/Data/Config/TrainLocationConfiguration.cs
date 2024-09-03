using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TrainLocationConfiguration : IEntityTypeConfiguration<TrainLocation>
{
    public void Configure(EntityTypeBuilder<TrainLocation> builder)
    {
        _ = builder.Property(p => p.UID).HasColumnType("nvarchar").HasMaxLength(4);
        _ = builder.Property(p => p.Latitude).HasPrecision(18, 11).IsRequired();
        _ = builder.Property(p => p.Longitude).HasPrecision(18, 11).IsRequired();
        _ = builder.Property(p => p.TrainNr).IsRequired();
        _ = builder.Property(p => p.TravelDate).HasColumnType("datetime").IsRequired();
    }
}
