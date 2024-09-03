using Core.Entities.Push;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class PushRegistrationConfiguration : IEntityTypeConfiguration<PushRegistration>
{
    public void Configure(EntityTypeBuilder<PushRegistration> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.Id).UseIdentityColumn(1000, 1);
        _ = builder.Property(p => p.TokenId).IsRequired().HasMaxLength(10000);
        _ = builder.Property(p => p.HWId).IsRequired().HasMaxLength(1000);
        _ = builder.Property(p => p.RegistrationDate).ValueGeneratedOnAdd().HasDefaultValueSql("[dbo].[dReturnDate](getdate())");
        _ = builder.Property(p => p.State).ValueGeneratedOnAdd().HasDefaultValue(1);
        _ = builder.Property(p => p.RefreshDate).HasDefaultValue("0001-01-01T00:00:00.0000000");

    }
}
