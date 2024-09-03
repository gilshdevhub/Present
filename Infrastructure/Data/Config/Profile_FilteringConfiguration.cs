using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities.PriceEngine;

namespace Infrastructure.Data.Config;

public class Profile_FilteringConfiguration : IEntityTypeConfiguration<Profile_Filtering>
{
    public void Configure(EntityTypeBuilder<Profile_Filtering> builder)
    {
        _ = builder.HasKey(p => new { p.Request_Id, p.Profile_Id });
        _ = builder.Property(p => p.Heb_Profile_Desc).HasColumnType("nvarchar(100)").HasMaxLength(100).IsRequired();
        _ = builder.Property(p => p.ModificationDate).IsRequired();
    }
}
