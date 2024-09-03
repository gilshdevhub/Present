using Core.Entities.PisData;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class PisDataService : IPisData
{
    private readonly ICacheService _cacheService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public PisDataService(ICacheService cacheService, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
    {
        _cacheService = cacheService;
        _webHostEnvironment = webHostEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<Siri> GetPisDataAsync()
    {
        Siri result = await _cacheService.GetAsync<Siri>(CacheKeys.PisSiri).ConfigureAwait(false);
        return result;
    }

    public async Task<IEnumerable<VehicleActivity>> GetPisDataByDestinationRefAsync(int DestinationRef)
    {
        IEnumerable<VehicleActivity> result = (await _cacheService.GetAsync<Siri>(CacheKeys.PisSiri).ConfigureAwait(false))
            .ServiceDelivery.VehicleMonitoringDelivery.VehicleActivity.Where(x => x.MonitoredVehicleJourney.DestinationRef == DestinationRef);
        return result;
    }

    public async Task<IEnumerable<VehicleActivity>> GetPisDataByOriginRefAsync(int OriginRef)
    {
        IEnumerable<VehicleActivity> result = (await _cacheService.GetAsync<Siri>(CacheKeys.PisSiri).ConfigureAwait(false))
            .ServiceDelivery.VehicleMonitoringDelivery.VehicleActivity.Where(x => x.MonitoredVehicleJourney.OriginRef == OriginRef);
        return result;
    }

    public async Task<IEnumerable<VehicleActivity>> GetPisDataByStopPointRefAsync(int StopPointRef)
    {
        IEnumerable<VehicleActivity> result = (await _cacheService.GetAsync<Siri>(CacheKeys.PisSiri).ConfigureAwait(false))
           .ServiceDelivery.VehicleMonitoringDelivery.VehicleActivity.Where(p =>
                p.MonitoredVehicleJourney.PreviousCalls.PreviousCall.All(i => i.StopPointRef == StopPointRef)
            ).ToList();

        return result;
    }

    public async Task<IEnumerable<VehicleActivity>> GetPisDataByStationIdAsync(int StationId)
    {
        IEnumerable<VehicleActivity> result = (await _cacheService.GetAsync<Siri>(CacheKeys.PisSiri).ConfigureAwait(false))
           .ServiceDelivery.VehicleMonitoringDelivery.VehicleActivity.Where(p =>
                               p.MonitoredVehicleJourney.MonitoredCall.StopPointRef == StationId && p.MonitoredVehicleJourney.MonitoredCall.VehicleAtStop
           ||
                               !p.MonitoredVehicleJourney.MonitoredCall.VehicleAtStop && p.MonitoredVehicleJourney.OnwardCalls.OnwardCall.Any(x => x.StopPointRef == StationId)//.OnwardCall.First().StopPointRef == StationId
           ).ToList();
        return result;


    }

    public async Task<IEnumerable<VehicleActivity>> GetPisDataByVehicleRefAsync(int VehicleRef)
    {
        IEnumerable<VehicleActivity> result = (await _cacheService.GetAsync<Siri>(CacheKeys.PisSiri).ConfigureAwait(false))
            .ServiceDelivery.VehicleMonitoringDelivery.VehicleActivity.Where(x => x.MonitoredVehicleJourney.VehicleRef == VehicleRef && x.MonitoredVehicleJourney.MonitoredCall.VehicleAtStop);
        return result;
    }

    public async Task<bool> SetPisDataCacheAsync(Siri PisData)
    {
              
              
                            
              
       
                            
                                                  bool result = false;
        try
        {
            var siri = await _cacheService.GetAsync<Siri>(CacheKeys.PisSiri).ConfigureAwait(false);
            if (siri == null)
            {
                await _cacheService.SetAsync<Siri>(CacheKeys.PisSiri, PisData).ConfigureAwait(false);
            }
            else
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.PisSiri).ConfigureAwait(false);
                await _cacheService.SetAsync<Siri>(CacheKeys.PisSiri, PisData).ConfigureAwait(false);
            }
            result = true;
        }
        catch (Exception ex)
        {
            throw;
        }
        return result;
    }

}
