using Core.Entities.Notifications;
using Core.Entities.SpeakerTimer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class DiscussionConfiguration : IEntityTypeConfiguration<Discussion>
{
    public void Configure(EntityTypeBuilder<Discussion> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Name).HasMaxLength(50).IsRequired();
        _ = builder.Property(p => p.Duration).IsRequired();
        _ = builder.Property(p => p.Date).IsRequired();
        _ = builder.HasOne(p => p.Owner).WithOne(p => p.Discussion).OnDelete(DeleteBehavior.Cascade);
        _ = builder.HasMany(p => p.Participant).WithOne(p => p.Discussion).OnDelete(DeleteBehavior.Cascade);
    }
}
