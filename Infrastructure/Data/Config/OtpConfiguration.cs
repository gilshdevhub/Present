using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class OtpConfiguration : IEntityTypeConfiguration<Otp>
{
    public void Configure(EntityTypeBuilder<Otp> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.OtpCode).HasMaxLength(50).IsRequired();
        _ = builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(50);
        _ = builder.Property(p => p.SystemName).IsRequired().HasMaxLength(200);
        _ = builder.Property(p => p.CreationDate).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("[dbo].[dReturnDate](getdate())");
    }
}
