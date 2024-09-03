using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class MetropolinConfiguration : IEntityTypeConfiguration<Metropolin>
{
    public void Configure(EntityTypeBuilder<Metropolin> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Id).ValueGeneratedNever();
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        _ = builder.HasMany(p => p.Stations).WithOne(p => p.Metropolin).OnDelete(DeleteBehavior.NoAction);
    }
}
