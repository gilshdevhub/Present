using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class SynonymConfiguration : IEntityTypeConfiguration<Synonym>
{
    public void Configure(EntityTypeBuilder<Synonym> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.StationId).IsRequired();
        _ = builder.Property(p => p.LanguageId).IsRequired();
        _ = builder.Property(p => p.SynonymName).IsRequired().HasMaxLength(200);
    }
}
