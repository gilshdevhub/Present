using Core.Entities.Stations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

internal class StationImageCompleteConfiguration : IEntityTypeConfiguration<StationImageComplete>
{
    public void Configure(EntityTypeBuilder<StationImageComplete> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Code).IsRequired();
        _ = builder.Property(p => p.CodeAR).IsRequired(false);
        _ = builder.Property(p => p.CodeEn).IsRequired(false);
        _ = builder.Property(p => p.CodeHe).IsRequired(false);
        _ = builder.Property(p => p.CodeRu).IsRequired(false);
    }
}
