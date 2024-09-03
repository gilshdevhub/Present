using Core.Entities.Identity;
using Core.Entities.MotUpdates;
using Core.Entities.MyTravel;
using Core.Entities.PagesManagement;
using Core.Entities.Push;
using Core.Entities.Stations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;

public class SpecialDbContext : DbContext
{
    public SpecialDbContext(DbContextOptions<SpecialDbContext> options) : base(options)
    {
    }
               
                   public DbSet<PushNotificationAndRegistrationIds> PushNotificationAndRegistrationIds { get; set; }
    public DbSet<PagesRoutesSP> Access { get; set; }
    public DbSet<PageResponse> PagesPerUsers { get; set; }
    public DbSet<PushNotificationsByDate> PushNotificationsByDate { get; set; }
    public DbSet<PushNotificationsByWeekDay> PushNotificationsByWeekDay { get; set; }
    public DbSet<StationImage> StationImageNames { get; set; }
    public DbSet<PublicTransportStations> PublicTransportStations { get; set; }
    public DbSet<BusTripHeadSignsDto> BusTripHeadSigns { get; set; }
    public DbSet<BusStopTimesDto> BusStopTimes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}