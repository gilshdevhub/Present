using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.ManagmentLogger;

namespace Infrastructure.Data.Config;

public class ManagmentLogConfiguration : IEntityTypeConfiguration<ManagmentLog>
{
    public void Configure(EntityTypeBuilder<ManagmentLog> builder)
    {
        _ = builder.HasKey(x => x.Id);
        _ = builder.Property(p => p.Title).HasMaxLength(100);
        _ = builder.Property(p => p.User).IsRequired().HasMaxLength(255);
        _ = builder.Property(p => p.EventType).IsRequired();
        _ = builder.Property(p => p.LogTime).IsRequired().ValueGeneratedOnAdd();
        _ = builder.Property(p => p.AdditionalData).IsRequired(false).HasMaxLength(2000);
    }
}
