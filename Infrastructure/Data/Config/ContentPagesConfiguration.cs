using Core.Entities.ContentPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;
public class ContentPagesConfiguration : IEntityTypeConfiguration<ContentPages>
{
    public void Configure(EntityTypeBuilder<ContentPages> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired().UseIdentityColumn(1, 1);
        builder.Property(p => p.Header);
        builder.Property(p => p.Content).IsRequired();
        builder.Property(p => p.Footer);
        builder.Property(p => p.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("[dbo].[dReturnDate](getdate())");
        builder.Property(p => p.LastUpdate).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("[dbo].[dReturnDate](getdate())");
    }
}

