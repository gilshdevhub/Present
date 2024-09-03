﻿using Core.Entities.AppMessages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class PopUpMessagesConfiguration : IEntityTypeConfiguration<PopUpMessages>
{
    public void Configure(EntityTypeBuilder<PopUpMessages> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.PageTypeId).IsRequired();
        _ = builder.Property(p => p.MessageBodyHebrew).IsRequired();
        _ = builder.Property(p => p.MessageBodyEnglish);
        _ = builder.Property(p => p.MessageBodyArabic);
        _ = builder.Property(p => p.MessageBodyRussian);
        _ = builder.Property(p => p.StartDate).IsRequired();
        _ = builder.Property(p => p.EndDate).IsRequired();
        _ = builder.Property(p => p.isActive).IsRequired();
        _ = builder.Property(p => p.SystemTypeId);
        _ = builder.Property(p => p.TitleHebrew).IsRequired();
        _ = builder.Property(p => p.TitleEnglish).IsRequired();
        _ = builder.Property(p => p.TitleRussian).IsRequired();
        _ = builder.Property(p => p.TitleArabic).IsRequired();
        _ = builder.Property(p => p.StationsIds).HasMaxLength(350);
    }
}
