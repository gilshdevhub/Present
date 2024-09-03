using Core.Entities.Push;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class PushRoutingConfiguration : IEntityTypeConfiguration<PushRouting>
{
    public void Configure(EntityTypeBuilder<PushRouting> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.CreatedDate).ValueGeneratedOnAdd().HasDefaultValueSql("[dbo].[dReturnDate](getdate())");
        _ = builder.Property(p => p.State).ValueGeneratedOnAdd().HasDefaultValue(1);
        _ = builder.Property(p => p.PermanentRegistration).IsRequired();

        _ = builder.HasMany(p => p.PushNotificationsByDate).WithOne(p => p.PushRouting).OnDelete(DeleteBehavior.Cascade);
        _ = builder.HasMany(p => p.PushNotificationsByWeekDay).WithOne(p => p.PushRouting).OnDelete(DeleteBehavior.Cascade);
    }
}
