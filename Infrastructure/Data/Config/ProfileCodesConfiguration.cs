using Core.Entities.Fares;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ProfileCodesConfiguration : IEntityTypeConfiguration<ProfileCodes>
{
    public void Configure(EntityTypeBuilder<ProfileCodes> builder)
    {
        _ = builder.HasKey(p => p.SmartCard_Profile_Code);
        _ = builder.Property(p => p.Profile_Name_He).HasMaxLength(256);
        _ = builder.Property(p => p.Profile_Name_Ru).HasMaxLength(256);
        _ = builder.Property(p => p.Profile_Name_En).HasMaxLength(256);
        _ = builder.Property(p => p.Profile_Name_Ar).HasMaxLength(256);
        _ = builder.Property(p => p.Profile_Magnetic);
    }
}
