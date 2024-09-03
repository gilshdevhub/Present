using Core.Entities;
using Core.Entities.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class BackgroundConfiguration : IEntityTypeConfiguration<BackgroundImage>
{
    public void Configure(EntityTypeBuilder<BackgroundImage> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.From);
        _ = builder.Property(p => p.Untill);
        _ = builder.Property(p => p.Name).HasMaxLength(50);
        _ = builder.Property(p => p.Decription).HasMaxLength(50);
        _ = builder.Property(p => p.Width);
        _ = builder.Property(p => p.Height);
        _ = builder.Property(p => p.Weight);
        _ = builder.Property(p => p.IsTempExists);

    }
}
