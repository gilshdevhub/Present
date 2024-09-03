using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TypesConfiguration : IEntityTypeConfiguration<TenderTypes>
{
    public void Configure(EntityTypeBuilder<TenderTypes> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Type).IsRequired();
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
    }
}
