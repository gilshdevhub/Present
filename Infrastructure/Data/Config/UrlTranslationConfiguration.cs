using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class URLTranslationConfiguration : IEntityTypeConfiguration<URLTranslation>
{
    public void Configure(EntityTypeBuilder<URLTranslation> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Key).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.SystemTypeId).IsRequired();
        _ = builder.Property(p => p.Hebrew).HasMaxLength(3000);
        _ = builder.Property(p => p.English).HasMaxLength(3000);
        _ = builder.Property(p => p.Russian).HasMaxLength(3000);
        _ = builder.Property(p => p.Arabic).HasMaxLength(3000);
        _ = builder.Property(p => p.IsActive).IsRequired();
    }
}
