using Core.Entities.Messenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class SentMessageOldConfiguration : IEntityTypeConfiguration<SentMessageOld>
{
    public void Configure(EntityTypeBuilder<SentMessageOld> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.MessageType).IsRequired();
        _ = builder.Property(p => p.SentDate).IsRequired();
        _ = builder.Property(p => p.Message).IsRequired().HasMaxLength(5000);
        _ = builder.Property(p => p.MessageTypeInfo).IsRequired().HasMaxLength(10000);
        _ = builder.Property(p => p.State).ValueGeneratedOnAdd().HasDefaultValue(1);
    }
}
