using Core.Config;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class StationGateService : IStationGateService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly RailUpdatesConfig _railUpdatesConfig;
    private readonly IHttpClientService _httpClient;
    private readonly IStationsService _stationsService;

    public StationGateService(RailDbContext context, ICacheService cacheService, IOptions<RailUpdatesConfig> railUpdatesConfig, IHttpClientService httpClient, IStationsService stationsService)

    {
        _context = context;
        _cacheService = cacheService;
        _railUpdatesConfig = railUpdatesConfig.Value;
        _httpClient = httpClient;
        _stationsService = stationsService;
    }

    public async Task<IEnumerable<StationGateDto>> GetStationGateAsync()
    {
        IEnumerable<StationGateDto> stationGate = await GeStationGatesNoCache().ConfigureAwait(false);

        return stationGate;
    }
    private async Task<IEnumerable<StationGateDto>> GeStationGatesNoCache()
    {
        IEnumerable<StationInfoTranslation> stationInfoTranslation = _context.StationInfoTranslation;
        IEnumerable<StationGateDto> stationGate = await _context.StationGate.Join(_context.Stations,
                                                           x => x.StationId,
                                                           y => y.StationId,
                                                           (x, y) =>
                                                           new
                                                           { gates = x, stations = y })
                                                       .Join(stationInfoTranslation,

                                                               x => new { p = x.gates.StationId, q = x.gates.GateNameTranslationKey },
                                                               y => new { p = y.StationId, q = y.Key },
                                                               (x, y) => new StationGateDto
                                                               {

                                                                   GateClosed = x.gates.GateClosed,
                                                                   GateClosedUntill = x.gates.GateClosedUntill,
                                                                   GateLatitude = x.gates.GateLatitude,
                                                                   GateLontitude = x.gates.GateLontitude,
                                                                   GateNameHebrew = y.Hebrew,
                                                                   GateNameEnglish = y.English,
                                                                   GateNameArabic = y.Arabic,
                                                                   GateNameRussian = y.Russian,
                                                                   GateName = y,
                                                                   GateAddress = x.gates.GateAddressTranslationKey != null ? stationInfoTranslation.Where(z => z.Key == x.gates.GateAddressTranslationKey).FirstOrDefault() : null,
                                                                   GateOrder = x.gates.GateOrder,
                                                                   GateParking = x.gates.GateParking,
                                                                   StationGateId = x.gates.StationGateId,
                                                                   StationId = x.stations.StationId,
                                                                   StationName = x.stations.HebrewName,
                                                                   GateNameTranslationKey = y.Key,
                                                                   StationGateServices = x.gates.StationGateServices.ToArray()
                                                               })
                 .ToArrayAsync().ConfigureAwait(false);

        foreach (var item in stationGate)
        {
            if (item.GateAddress != null)
            {
                item.GateAddressHebrew = item.GateAddress.Hebrew;
                item.GateAddressArabic = item.GateAddress.Arabic;
                item.GateAddressRussian = item.GateAddress.Russian;
                item.GateAddressEnglish = item.GateAddress.English;
            }
        }


        return stationGate;
    }


    public async Task<IEnumerable<StationServices>> GetStationServicesAsync()
    {
        IEnumerable<StationServices> stationServices = await GetStationCacheServicesAsync();
        return stationServices;
    }
    private async Task<IEnumerable<StationServices>> GetStationCacheServicesAsync()
    {
        IEnumerable<StationServices> stationServices = await _cacheService.GetAsync<IEnumerable<StationServices>>(CacheKeys.StationServices).ConfigureAwait(false);
        await _cacheService.RemoveCacheItemAsync(CacheKeys.Stations).ConfigureAwait(false);
        if (stationServices == null || stationServices.Count() <= 0)
        {
            stationServices = await _context.StationServices.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<StationServices>>(CacheKeys.StationServices, stationServices).ConfigureAwait(false);
        }
        return stationServices;
    }
    #region AddDeleteUpdate
    public async Task<bool> InsertStationGateAsync(StationGate stationGateToInsert,
        IEnumerable<StationInfoTranslation> stationInfoTranslations)
    {
        foreach (var item in stationInfoTranslations)
        {
            _ =
                   _context.StationInfoTranslation.Add(item);
        }

        EntityEntry<StationGate> entity = _context.StationGate.Add(stationGateToInsert);
        _ = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;

        foreach (var item in _context.StationServices)
        {
            StationGateServices stationGateServices = new(entity.Entity.StationGateId, item.ServiceId, false);
            _ = _context.StationGateServices.Add(stationGateServices);
        }




        List<StationGate> list = await _context.StationGate.Where(s => s.StationId == stationGateToInsert.StationId).ToListAsync().ConfigureAwait(false);
        list.Add(stationGateToInsert);
        StationInfo stationInfo = await CalculateStationinIsClosed(list, stationGateToInsert.StationId);
        EntityEntry<StationInfo> stationInfoEntity = _context.StationInfo.Update(stationInfo);
        bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;

        if (success)
        {

            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationGate);
            IEnumerable<StationGateDto> stationGate = await GeStationGatesNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<StationGateDto>>(CacheKeys.StationGate, stationGate).ConfigureAwait(false);
            _ = await GetStationCacheServicesAsync().ConfigureAwait(false);
        }


        return success;
    }
    public async Task<IEnumerable<Station>> DeleteStationGateAsync(int StationGateId)
    {
        StationGate stationsGate = await _context.StationGate.SingleOrDefaultAsync(p => p.StationGateId == StationGateId).ConfigureAwait(false);
        IEnumerable<Station> stations = null;
        if (stationsGate != null)
        {
            IEnumerable<StationInfoTranslation> stationInfoTranslation = _context.StationInfoTranslation.Where(x => x.Key == stationsGate.GateNameTranslationKey);
            if (stationInfoTranslation != null)
                foreach (var gateTranslation in stationInfoTranslation)
                {
                    _ = _context.StationInfoTranslation.Remove(gateTranslation);
                }
            _ = _context.StationGate.Remove(stationsGate);
            bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            List<StationGate> list = await _context.StationGate.Where(s => s.StationId == stationsGate.StationId).ToListAsync().ConfigureAwait(false);
            list.Remove(stationsGate);
            StationInfo stationInfo = await CalculateStationinIsClosed(list, stationsGate.StationId);
            EntityEntry<StationInfo> stationInfoEntity = _context.StationInfo.Update(stationInfo);

            /*********************  בשביל לעדכן את ה STATION INFO TRNASLATION *******************************************/
            stations = await _stationsService.GetStationsNoCache();
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.Stations);
            await _cacheService.SetAsync<IEnumerable<Station>>(CacheKeys.Stations, stations).ConfigureAwait(false);
            /*********************  בשביל לעדכן את ה STATION INFO TRNASLATION *******************************************/



            success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationGate);
                IEnumerable<StationGateDto> stationGate = await GeStationGatesNoCache().ConfigureAwait(false);
                await _cacheService.SetAsync<IEnumerable<StationGateDto>>(CacheKeys.StationGate, stationGate).ConfigureAwait(false);
                _ = await GetStationCacheServicesAsync().ConfigureAwait(false);
            }
        }
        return stations;
    }
    private async Task<int> UpdateSingleGateServices(StationGate stationGateToUpdate)
    {
        List<StationGateServices> StationGateServices = _context.StationGateServices
                                                    .Where(x => x.StationGateId == stationGateToUpdate.StationGateId).ToList();
        var updateItem = stationGateToUpdate.StationGateServices.ToList().Except(StationGateServices).OrderBy(x => x.ServiceId).ToList();
        var dbItem = StationGateServices.Except(stationGateToUpdate.StationGateServices.ToList()).OrderBy(x => x.ServiceId).ToList();

        var addToDb = updateItem.Except(updateItem.Where(o => dbItem.Select(s => s.ServiceId).Contains(o.ServiceId)));
        var removeFromDb = dbItem.Except(dbItem.Where(o => updateItem.Select(s => s.ServiceId).Contains(o.ServiceId)));
        int gatesuccess = -1;
        foreach (var gate in addToDb)
        {
            EntityEntry<StationGateServices> gateentity = _context.StationGateServices.Add(gate);
            gateentity.State = EntityState.Added;
            gatesuccess = await _context.SaveChangesAsync().ConfigureAwait(false);// > 0;
            if (gatesuccess > 0)
            {
                continue;
            }
        }
        foreach (var gate in removeFromDb)
        {
            StationGateServices item = _context.StationGateServices.SingleOrDefault(p => p.ServiceId == gate.ServiceId && p.StationGateId == gate.StationGateId);
            _ = _context.StationGateServices.Remove(item);
            gatesuccess = await _context.SaveChangesAsync().ConfigureAwait(false);// > 0;
            if (gatesuccess > 0)
            {
                continue;
            }
        }
        return gatesuccess;
    }
    public async Task<bool> UpdateStationGateAsync(StationGate stationGateToUpdate, IEnumerable<StationInfoTranslation> stationInfoTranslations)
    {
        bool success;
        EntityEntry<StationGate> entity = _context.StationGate.Update(stationGateToUpdate);
        if (entity == null)
        {
            entity.State = EntityState.Unchanged;
        }
        else
        {
            entity.State = EntityState.Modified;
            _ = await UpdateSingleGateServices(stationGateToUpdate).ConfigureAwait(false);
            success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        if (stationInfoTranslations != null)
        {
            foreach (var item in stationInfoTranslations)
            {
                EntityEntry<StationInfoTranslation> entityStationInfoTranslation =
                       _context.StationInfoTranslation.Update(item);
            }
        }
        List<StationGate> list = await _context.StationGate.Where(s => s.StationId == stationGateToUpdate.StationId).ToListAsync().ConfigureAwait(false);
        success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;

        StationInfo stationInfo = await CalculateStationinIsClosed(list, stationGateToUpdate.StationId);
        EntityEntry<StationInfo> stationInfoEntity = _context.StationInfo.Update(stationInfo);

        success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.StationInfo);
            IEnumerable<StationInfo> stationsInfo = await _context.StationInfo.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<StationInfo>>(CacheKeys.StationInfo, stationsInfo).ConfigureAwait(false);

        }

        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.StationGate);
            IEnumerable<StationGateDto> stationGate = await GeStationGatesNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<StationGateDto>>(CacheKeys.StationGate, stationGate).ConfigureAwait(false);
            _ = await GetStationCacheServicesAsync().ConfigureAwait(false);
        }



        return success;
    }
    private async Task<StationInfo> CalculateStationinIsClosed(List<StationGate> list, int StationId)
    {
        StationInfo stationInfo = await _context.StationInfo.Where(s => s.StationId == StationId).FirstOrDefaultAsync().ConfigureAwait(false);
               int count = 0;
        DateTime date = new DateTime(0001, 01, 01);
        foreach (var item in list)
        {
            if (item.GateClosed && (item.GateClosedUntill > DateTime.Now || item.GateClosedUntill == new DateTime(0001, 01, 01)))
            {
                count++;
                if (
                     (item.GateClosedUntill < date && item.GateClosedUntill != new DateTime(0001, 01, 01))
                    || (date == new DateTime(0001, 01, 01) && item.GateClosedUntill > date)
                    )
                {
                    date = item.GateClosedUntill;
                }
            }

        }
        if (count == list.Count)
        {
            stationInfo.StationIsClosed = true;
            stationInfo.StationIsClosedUntill = date;
        }
        else
        {
            stationInfo.StationIsClosed = false;
            stationInfo.StationIsClosedUntill = new DateTime(0001, 01, 01);
        }
        return stationInfo;
    }

    #endregion AddDeleteUpdate
}
