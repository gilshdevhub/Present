using Core.Entities.Surveys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

internal class SurveysResultsConfiguration : IEntityTypeConfiguration<SurveysResults>
{
    public void Configure(EntityTypeBuilder<SurveysResults> builder)
    {
        _ = builder.Property(p => p.TimeStamp).IsRequired();
        _ = builder.Property(p => p.CustID).IsRequired().HasMaxLength(12).IsUnicode();
        _ = builder.Property(p => p.SystemTypeId).IsRequired();
        _ = builder.Property(p => p.Score).IsRequired();
        _ = builder.Property(p => p.SurveyId).IsRequired();
        _ = builder.HasKey(x => new { x.CustID, x.TimeStamp });
    }
}
