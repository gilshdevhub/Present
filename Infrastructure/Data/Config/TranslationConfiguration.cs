using Core.Entities.Translation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
{
    public void Configure(EntityTypeBuilder<Translation> builder)
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
