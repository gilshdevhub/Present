using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TenderDocumentsConfiguration : IEntityTypeConfiguration<TenderDocuments>
{
    public void Configure(EntityTypeBuilder<TenderDocuments> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.DocName).HasMaxLength(256).IsRequired(false);
        _ = builder.Property(p => p.DocType);
        _ = builder.Property(p => p.DocDisplay).HasMaxLength(256).IsRequired(false);
        _ = builder.HasOne(p => p.Tenders).WithMany(p => p.Documentation).HasForeignKey(p => p.TendersId).OnDelete(DeleteBehavior.ClientSetNull).IsRequired(false);
        _ = builder.HasOne(p => p.SingleSupplier).WithMany(p => p.Documentation).HasForeignKey(p => p.SingleSupplierId).OnDelete(DeleteBehavior.ClientSetNull).IsRequired(false);
        _ = builder.HasOne(p => p.ExemptionNotices).WithMany(p => p.Documentation).HasForeignKey(p => p.ExemptionNoticesId).OnDelete(DeleteBehavior.ClientSetNull).IsRequired(false);
        _ = builder.HasOne(p => p.PlanningAndRates).WithMany(p => p.Documentation).HasForeignKey(p => p.PlanningAndRatesId).OnDelete(DeleteBehavior.ClientSetNull).IsRequired(false);
    }
}
