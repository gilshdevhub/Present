using Core.Entities.Notifications;
using Core.Entities.SpeakerTimer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class TimerUserConfiguration : IEntityTypeConfiguration<TimerUser>
{
    public void Configure(EntityTypeBuilder<TimerUser> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Email).HasMaxLength(255).IsRequired();
        _ = builder.Property(p => p.UserName).IsRequired();
        _ = builder.Property(p => p.Password).IsRequired();
        _ = builder.HasOne(p => p.TimerRole).WithOne(p => p.TimerUsers).HasForeignKey<TimerUser>(p=>p.RoleId).OnDelete(DeleteBehavior.ClientSetNull).IsRequired();

    }
}
