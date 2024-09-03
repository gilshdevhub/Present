using Core.Entities.PriceEngine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class PriceNotesConfiguration : IEntityTypeConfiguration<PriceNotes>
{
    public void Configure(EntityTypeBuilder<PriceNotes> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Profile_Id);
        _ = builder.Property(p => p.PriceNoteHe).HasMaxLength(300);
        _ = builder.Property(p => p.PriceNoteEn).HasMaxLength(300);
        _ = builder.Property(p => p.PriceNoteAr).HasMaxLength(300);
        _ = builder.Property(p => p.PriceNoteRu).HasMaxLength(300);
        _ = builder.Property(p => p.SingleNote);
        _ = builder.Property(p => p.DayNote);
        _ = builder.Property(p => p.MonthNote);
    }
}