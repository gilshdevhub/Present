using Core.Entities.Fares;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class FaresVersionsConfiguration : IEntityTypeConfiguration<FaresVersions>
{
    public void Configure(EntityTypeBuilder<FaresVersions> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Version);
        _ = builder.Property(p => p.StartDate);
        _ = builder.Property(p => p.EndDate);
    }
}
