using Core.Entities.Notifications;
using Core.Entities.SpeakerTimer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TimerRoleConfiguration : IEntityTypeConfiguration<TimerRole>
{
    public void Configure(EntityTypeBuilder<TimerRole> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Name).IsRequired();
               _ = builder.HasOne(p => p.TimerUsers).WithOne(p => p.TimerRole).HasForeignKey<TimerUser>(p => p.RoleId).OnDelete(DeleteBehavior.ClientSetNull).IsRequired(false);
    }
}
