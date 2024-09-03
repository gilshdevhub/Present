using Core.Entities.Notifications;
using Core.Entities.SpeakerTimer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        _ = builder.HasKey(p => p.Id);
              _ = builder.Property(p => p.Email).HasMaxLength(255);
        _ = builder.Property(p => p.Phone).IsRequired();
        _ = builder.Property(p => p.Duration).IsRequired();
        _ = builder.Property(p => p.Role).IsRequired();
        _ = builder.Property(p => p.Name).IsRequired();
        _ = builder.Property(p => p.JobRole).IsRequired();
    }
}
