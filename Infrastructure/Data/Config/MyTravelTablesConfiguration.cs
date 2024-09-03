using Core.Entities.MyTravel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class EmployeeDataFVConfiguration : IEntityTypeConfiguration<EmployeeDataFV>
{
    public void Configure(EntityTypeBuilder<EmployeeDataFV> builder)
    {

        _ = builder.HasKey(nameof(EmployeeDataFV.NMBRAK), nameof(EmployeeDataFV.DTRKNS), nameof(EmployeeDataFV.OVOVDNUM));
        _ = builder.HasOne(p => p.TrainData).WithMany(p => p.Employees)
            .HasForeignKey(nameof(EmployeeDataFV.NMBRAK), nameof(EmployeeDataFV.DTRKNS))
            .OnDelete(DeleteBehavior.NoAction).IsRequired(false);
        _ = builder.Property(p => p.OVMNEL).HasDefaultValue(0);


    }
}

public class EngineDataFVConfiguration : IEntityTypeConfiguration<EngineDataFV>
{
    public void Configure(EntityTypeBuilder<EngineDataFV> builder)
    {

        _ = builder.HasKey(nameof(EngineDataFV.NMBRAK), nameof(EngineDataFV.DTRKNS), nameof(EngineDataFV.KTKTRSG), nameof(EngineDataFV.KTKTRKB), nameof(EngineDataFV.KTKTRNO));
        _ = builder.HasOne(p => p.TrainData).WithMany(p => p.Engines)
          .HasForeignKey(nameof(EngineDataFV.NMBRAK), nameof(EngineDataFV.DTRKNS))
          .OnDelete(DeleteBehavior.NoAction).IsRequired(false);
    }
}


public class HandicapDataFVConfiguration : IEntityTypeConfiguration<HandicapDataFV>
{
    public void Configure(EntityTypeBuilder<HandicapDataFV> builder)
    {

               _ = builder.HasKey(nameof(TrainDataFV.NMBRAK), nameof(TrainDataFV.DTRKNS));
        _ = builder.HasOne(p => p.TrainData).WithMany(p => p.Handicaps)
            .HasForeignKey(nameof(HandicapDataFV.NMBRAK), nameof(HandicapDataFV.DTRKNS))
            .OnDelete(DeleteBehavior.NoAction).IsRequired();
        _ = builder.Property(p => p.HCPRK1).HasDefaultValue(0);
        _ = builder.Property(p => p.HCPRK2).HasDefaultValue(0);
        _ = builder.Property(p => p.HCPTHCHG).HasDefaultValue(0);
    }
}


public class StationDataFVConfiguration : IEntityTypeConfiguration<StationDataFV>
{
    public void Configure(EntityTypeBuilder<StationDataFV> builder)
    {

        _ = builder.HasKey(nameof(StationDataFV.NMBRAK), nameof(StationDataFV.DTRKNS), nameof(StationDataFV.SHURA2));
        _ = builder.HasOne(p => p.VisaTimesData).WithMany(p => p.Stations)
           .HasForeignKey(nameof(StationDataFV.NMBRAK), nameof(StationDataFV.DTRKNS))
           .OnDelete(DeleteBehavior.NoAction).IsRequired();
        _ = builder.Property(p => p.ARR_DIFF).HasDefaultValue(0);
        _ = builder.Property(p => p.DEP_DIFF).HasDefaultValue(0);
    }
}


public class TrainDataFVConfiguration : IEntityTypeConfiguration<TrainDataFV>
{
    public void Configure(EntityTypeBuilder<TrainDataFV> builder)
    {

        _ = builder.HasKey(nameof(TrainDataFV.NMBRAK), nameof(TrainDataFV.DTRKNS));
        _ = builder.Property(p => p.TUDANO).HasDefaultValue(0);
    }
}


public class VisaTimesDataFVConfiguration : IEntityTypeConfiguration<VisaTimesDataFV>
{
    public void Configure(EntityTypeBuilder<VisaTimesDataFV> builder)
    {

        _ = builder.HasKey(nameof(VisaTimesDataFV.NMBRAK), nameof(VisaTimesDataFV.DTRKNS));
        _ = builder.HasOne(p => p.TrainData).WithOne(p => p.VISA_TIMES)
        .HasForeignKey<VisaTimesDataFV>(nameof(VisaTimesDataFV.NMBRAK), nameof(VisaTimesDataFV.DTRKNS)).IsRequired()
        .OnDelete(DeleteBehavior.NoAction).IsRequired();
        _ = builder.Property(p => p.ArrivalTomorrow).HasDefaultValue(0);
    }
}


public class WagonDataFVConfiguration : IEntityTypeConfiguration<WagonDataFV>
{
    public void Configure(EntityTypeBuilder<WagonDataFV> builder)
    {

        _ = builder.HasKey(nameof(WagonDataFV.NMBRAK), nameof(WagonDataFV.DTRKNS), nameof(WagonDataFV.SGKRON), nameof(WagonDataFV.KBKRON), nameof(WagonDataFV.KRSID));
        _ = builder.HasOne(p => p.TrainData).WithMany(p => p.Wagons)
          .HasForeignKey(nameof(WagonDataFV.NMBRAK), nameof(WagonDataFV.DTRKNS))
          .OnDelete(DeleteBehavior.NoAction).IsRequired();
        _ = builder.Property(p => p.TUDANO).HasDefaultValue(0);
    }
}



