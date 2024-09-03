using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class PlanningAndRatesConfiguration : IEntityTypeConfiguration<PlanningAndRates>
{
    public void Configure(EntityTypeBuilder<PlanningAndRates> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.SerialNumber);
        _ = builder.Property(p => p.PlanningAreas);
        _ = builder.Property(p => p.Subject);
        _ = builder.Property(p => p.UpdatingUser);
        _ = builder.Property(p => p.Language);
        _ = builder.Property(p => p.Domain);
        _ = builder.Property(p => p.TypeOfTender);
        _ = builder.Property(p => p.Page);
        _ = builder.Property(p => p.UpdateDate);
        _ = builder.HasOne(p => p.MailingList).WithOne(p => p.PlanningAndRates).HasForeignKey<MailingList>(p => p.PlanningAndRatesId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
    }
}
