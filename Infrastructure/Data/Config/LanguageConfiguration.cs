using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        _ = builder.HasMany(p => p.Synonyms).WithOne(p => p.Language).OnDelete(DeleteBehavior.NoAction);
    }
}
