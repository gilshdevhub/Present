using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class SingleSupplierConfiguration : IEntityTypeConfiguration<SingleSupplier>
{
    public void Configure(EntityTypeBuilder<SingleSupplier> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.PublishDate).IsRequired();
        _ = builder.Property(p => p.SupplierName).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.ReferentName).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.ReferentMail).IsRequired().HasMaxLength(256);
        _ = builder.Property(p => p.Subject).IsRequired(false).HasMaxLength(200);
        _ = builder.Property(p => p.Domain);
        _ = builder.Property(p => p.TypeOfTender);
        _ = builder.Property(p => p.Page);
        _ = builder.Property(p => p.Language).IsRequired().HasMaxLength(2);
        _ = builder.Property(p => p.UpdateDate).ValueGeneratedOnUpdate();
        _ = builder.Property(p => p.UpdatingUser).IsRequired().HasMaxLength(100);
        _ = builder.HasOne(p => p.MailingList).WithOne(p => p.SingleSupplier).HasForeignKey<MailingList>(p => p.SingleId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        _ = builder.HasMany(p => p.Documentation).WithOne(p => p.SingleSupplier).HasForeignKey(p => p.SingleSupplierId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
    }
}
