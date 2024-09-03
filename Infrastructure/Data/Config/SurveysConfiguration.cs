using Core.Entities.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

internal class SurveysConfiguration : IEntityTypeConfiguration<SurveysData>
{
    public void Configure(EntityTypeBuilder<SurveysData> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(100).IsUnicode();
        _ = builder.Property(p => p.Description).IsRequired(false).HasMaxLength(200).IsUnicode();
        _ = builder.Property(p => p.StartDate).IsRequired(false);
        _ = builder.Property(p => p.EndDate).IsRequired(false);
    }
}
