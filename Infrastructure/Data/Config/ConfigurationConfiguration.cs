using Core.Entities.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
{
    public void Configure(EntityTypeBuilder<Configuration> builder)
    {
        _ = builder.Property(p => p.Key).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.SystemTypeId).IsRequired();
        _ = builder.Property(p => p.Value).IsRequired().HasMaxLength(5000);
        _ = builder.Property(p => p.Description).IsRequired(false).HasMaxLength(3000);
    }
}
