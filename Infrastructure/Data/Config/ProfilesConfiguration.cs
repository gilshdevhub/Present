using Core.Entities.PriceEngine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ProfilesConfiguration : IEntityTypeConfiguration<Profiles>
{
    public void Configure(EntityTypeBuilder<Profiles> builder)
    {
        _ = builder.HasKey(p => p.Profile_Id);
        _ = builder.Property(p => p.Heb_Profile_Desc).HasMaxLength(100);
        _ = builder.Property(p => p.Eng_Desc).HasMaxLength(100);
        _ = builder.Property(p => p.Arb_Desc).HasMaxLength(100);
        _ = builder.Property(p => p.Rus_Desc).HasMaxLength(100);
        _ = builder.Property(p => p.From_Age);
        _ = builder.Property(p => p.To_Age);
        _ = builder.Property(p => p.Contracts_Discount_FL);
        _ = builder.Property(p => p.Accum_Discount_FL);
        _ = builder.Property(p => p.CreationDate);
    }
}