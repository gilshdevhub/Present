using Core.Entities.VersionCatalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class VersionCatalogConfiguration : IEntityTypeConfiguration<VersionCatalog>
{
    public void Configure(EntityTypeBuilder<VersionCatalog> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Id).ValueGeneratedNever();
        _ = builder.Property(p => p.Description).IsRequired().HasMaxLength(200);
        _ = builder.Property(p => p.VersionId).IsRequired();
    }
}
