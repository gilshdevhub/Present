using Core.Entities.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class SystemTypeConfiguration : IEntityTypeConfiguration<SystemType>
{
    public void Configure(EntityTypeBuilder<SystemType> builder)
    {
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
    }
}
