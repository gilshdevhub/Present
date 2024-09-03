using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TendersUpdatesConfiguration : IEntityTypeConfiguration<TendersUpdates>
{
    public void Configure(EntityTypeBuilder<TendersUpdates> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Guid).IsRequired();
        _ = builder.Property(p => p.Type).IsRequired();
        _ = builder.Property(p => p.PublishDate);
        _ = builder.Property(p => p.Domain);
        _ = builder.Property(p => p.TypeOfTender);
        _ = builder.Property(p => p.Page);
        _ = builder.Property(p => p.Step);
    }
}
