using Core.Entities.AppMessages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class WarningTypeConfiguration : IEntityTypeConfiguration<WarningType>
{
    public void Configure(EntityTypeBuilder<WarningType> builder)
    {
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
    }
}
