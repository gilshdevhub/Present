using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class VoucherErrorCodeConfiguration : IEntityTypeConfiguration<VoucherErrorCode>
{
    public void Configure(EntityTypeBuilder<VoucherErrorCode> builder)
    {
        _ = builder.HasKey(p => p.LegacyErrorCode);
        _ = builder.Property(p => p.Id).IsRequired().UseIdentityColumn();
        _ = builder.Property(p => p.LegacyErrorCode).IsRequired().HasMaxLength(50);
        _ = builder.Property(p => p.ErrorCode).IsRequired();
        _ = builder.Property(p => p.ErrorDescription).IsRequired().HasMaxLength(2000);
    }
}
