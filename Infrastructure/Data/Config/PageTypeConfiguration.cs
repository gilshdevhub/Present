using Core.Entities.AppMessages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class PageTypeConfiguration : IEntityTypeConfiguration<PageType>
{
    public void Configure(EntityTypeBuilder<PageType> builder)
    {
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
    }
}
