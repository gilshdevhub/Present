using Core.Entities.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ConfigurationParameterConfiguration : IEntityTypeConfiguration<ConfigurationParameter>
{
    public void Configure(EntityTypeBuilder<ConfigurationParameter> builder)
    {
        _ = builder.HasKey(p => p.Key);
               _ = builder.Property(p => p.Key).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.ValueMob).HasMaxLength(5000);
        _ = builder.Property(p => p.ValueWeb).HasMaxLength(5000);
        _ = builder.Property(p => p.Description).IsRequired(false).HasMaxLength(3000);
    }
}
