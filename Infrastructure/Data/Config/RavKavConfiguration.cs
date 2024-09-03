using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class RavKavConfiguration : IEntityTypeConfiguration<RavKav>
{
    public void Configure(EntityTypeBuilder<RavKav> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.RavKavNumber).IsRequired();
        _ = builder.Property(p => p.Amount).IsRequired();
        _ = builder.Property(p => p.Refundable).IsRequired();
    }
}
