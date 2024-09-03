using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationActivityTemplatesTypesConfiguration : IEntityTypeConfiguration<StationActivityTemplatesTypes>
{
    public void Configure(EntityTypeBuilder<StationActivityTemplatesTypes> builder)
    {
        _ = builder.HasKey(p => p.TemplateTypeId);
        _ = builder.Property(p => p.TemplateTypeName).HasMaxLength(50).IsRequired();
    }
}
