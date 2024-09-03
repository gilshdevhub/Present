using Core.Entities.Messenger;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class SentMessagesConfiguration : IEntityTypeConfiguration<SentMessages>
{
    public void Configure(EntityTypeBuilder<SentMessages> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.MessageType).IsRequired();
        _ = builder.Property(p => p.MessageTypeInfo).HasColumnType("varchar").IsRequired().HasMaxLength(5000);
        _ = builder.Property(p => p.MessageId).IsRequired();
        _ = builder.Property(p => p.ResponseStatus).IsRequired().HasMaxLength(256);
        _ = builder.Property(p => p.SentDate).IsRequired();
        _ = builder.Property(p => p.State).ValueGeneratedOnAdd().HasDefaultValue(1);
    }
}
