using Core.Entities.Messenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.MessageText).IsRequired().HasMaxLength(1024);
        _ = builder.HasMany(p => p.newSentMessages).WithOne(p => p.Message).OnDelete(DeleteBehavior.Cascade);
    }
}
