using Core.Entities.Fares;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class EttCodesConfiguration : IEntityTypeConfiguration<EttCodes>
{
    public void Configure(EntityTypeBuilder<EttCodes> builder)
    {
        _ = builder.HasKey(p => p.ETT_Code);
        _ = builder.Property(p => p.RelevantForMetropoline);
        _ = builder.Property(p => p.ETT_Name_Ar);
        _ = builder.Property(p => p.ETT_Name_En);
        _ = builder.Property(p => p.ETT_Name_He);
        _ = builder.Property(p => p.ETT_Name_Ru);
    }
}
