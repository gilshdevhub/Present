using Core.Entities.Vouchers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class StationGateServicesConfiguration : IEntityTypeConfiguration<StationGateServices>
{
    public void Configure(EntityTypeBuilder<StationGateServices> builder)
    {
        _ = builder.HasKey(nameof(StationGateServices.StationGateId), nameof(StationGateServices.ServiceId));
        _ = builder.Property(p => p.isServiceExist).IsRequired().HasDefaultValue(false);


    }
}
