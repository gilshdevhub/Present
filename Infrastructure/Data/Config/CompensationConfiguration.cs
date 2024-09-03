using Core.Entities.Compensation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

class CompensationConfiguration : IEntityTypeConfiguration<Compensation>
{
    public void Configure(EntityTypeBuilder<Compensation> builder)
    {
        _ = builder.HasKey(p => p.Id);
        _ = builder.Property(p => p.SmartCard).IsRequired();
        _ = builder.Property(p => p.RequestId).IsRequired();
        _ = builder.Property(p => p.PhoneNumber).HasMaxLength(10).IsRequired();
        _ = builder.Property(p => p.OriginStationId).IsRequired();
        _ = builder.Property(p => p.DestinationStationId).IsRequired();
        _ = builder.Property(p => p.CardNumber).HasMaxLength(20).IsRequired();
        _ = builder.Property(p => p.CardValidityFromDate).IsRequired();
        _ = builder.Property(p => p.CardValidityToDate).IsRequired();
        _ = builder.Property(p => p.CardrecievedDate).ValueGeneratedOnAdd().HasDefaultValueSql("([dbo].[dReturnDate](getdate()))");
        _ = builder.Property(p => p.QRCode).HasMaxLength(400).IsRequired();
    }
}
