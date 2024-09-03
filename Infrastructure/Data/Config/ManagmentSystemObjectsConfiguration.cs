using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ManagmentSystemObjectsConfiguration : IEntityTypeConfiguration<ManagmentSystemObjects>
{
    public void Configure(EntityTypeBuilder<ManagmentSystemObjects> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Id).IsRequired();
        _ = builder.Property(p => p.StringValue).IsRequired().HasMaxLength(256); ;
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.Description).IsRequired().HasMaxLength(256);
    }
}
