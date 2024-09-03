using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class MailingListConfiguration : IEntityTypeConfiguration<MailingList>
{
    public void Configure(EntityTypeBuilder<MailingList> builder)
    {
        _ = builder.HasKey(p => p.MailingListId);
        _ = builder.Property(p => p.Mails).IsRequired().HasMaxLength(512);
        _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(512);
        _ = builder.Property(p => p.Domain);
        _ = builder.Property(p => p.TypeOfTender);
        _ = builder.Property(p => p.Page);
        _ = builder.Property(p => p.Step);
    }
}
