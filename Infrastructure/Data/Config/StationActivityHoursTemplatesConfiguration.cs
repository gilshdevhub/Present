using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationActivityHoursTemplatesConfiguration : IEntityTypeConfiguration<StationActivityHoursTemplates>
{
    public void Configure(EntityTypeBuilder<StationActivityHoursTemplates> builder)
    {
        _ = builder.HasKey(p => p.TemplateId);
        _ = builder.HasIndex(p => p.TemplateName).IsUnique();
        _ = builder.Property(p => p.TemplateTypeId).IsRequired();
    }
}
