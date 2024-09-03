using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TenderConfiguration : IEntityTypeConfiguration<Tenders>
{
    public void Configure(EntityTypeBuilder<Tenders> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.PublishDate).IsRequired();
        _ = builder.Property(p => p.ReferentName).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.ReferentMail).IsRequired().HasMaxLength(256);
        _ = builder.Property(p => p.Domain);
        _ = builder.Property(p => p.TypeOfTender);
        _ = builder.Property(p => p.Page);
        _ = builder.Property(p => p.TenderNumber).IsRequired();
        _ = builder.Property(p => p.TenderName).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.ClarifyingDate).IsRequired(false);
        _ = builder.Property(p => p.BiddingDate).IsRequired(false);
        _ = builder.Property(p => p.WinningSupplier).IsRequired(false).HasMaxLength(512);
        _ = builder.Property(p => p.WinningDate).IsRequired(false);
        _ = builder.HasMany(p => p.Documentation).WithOne(p => p.Tenders).HasForeignKey(p => p.TendersId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        _ = builder.Property(p => p.Description).IsRequired(false).HasMaxLength(2000);
        _ = builder.Property(p => p.Biddings).IsRequired(false).HasMaxLength(512);
        _ = builder.Property(p => p.Language).IsRequired().HasMaxLength(2);
        _ = builder.Property(p => p.UpdateDate).ValueGeneratedOnUpdate();
        _ = builder.Property(p => p.UpdatingUser).IsRequired().HasMaxLength(100);
        _ = builder.Property(p => p.WaitingSupplier).IsRequired(false).HasMaxLength(512);
        _ = builder.HasOne(p => p.MailingList).WithOne(p => p.Tenders).HasForeignKey<MailingList>(p => p.TenderId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);
        _ = builder.Property(p => p.WinningAmount).IsRequired(false).HasMaxLength(256);
    }
}
