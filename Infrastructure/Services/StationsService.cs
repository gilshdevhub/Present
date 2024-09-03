using Core.Config;
using Core.Entities.RailUpdates;
using Core.Entities.Translation;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Globalization;

namespace Infrastructure.Services;

public class StationsService : IStationsService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly RailUpdatesConfig _railUpdatesConfig;
    private readonly IHttpClientService _httpClient;
    private readonly IRailUpdatesService _railUpdatesService;

    public StationsService(RailDbContext context, ICacheService cacheService, IOptions<RailUpdatesConfig> railUpdatesConfig,
                             IHttpClientService httpClient, IRailUpdatesService railUpdatesService)

    {
        _context = context;
        _cacheService = cacheService;

        _railUpdatesConfig = railUpdatesConfig.Value;
        _httpClient = httpClient;
        _railUpdatesService = railUpdatesService;
    }

    public async Task<Station> GetStationAsync(int id)
    {
        IEnumerable<Station> stations = await GetCacheStationsAsync().ConfigureAwait(false);
        Station station = stations.SingleOrDefault(p => p.StationId == id);
        return station;
    }

    public async Task<Station> GetStationNoCache(int id)
    {
        IEnumerable<Station> stations = await GetStationsNoCache().ConfigureAwait(false);
        Station station = stations.SingleOrDefault(p => p.StationId == id);
        return station;
    }

    public async Task<IEnumerable<Station>> GetStationsAsync()
    {
        return await GetCacheStationsAsync().ConfigureAwait(false);
    }
    public async Task<IEnumerable<Station>> GetStationsNoCache()
    {
        IEnumerable<Station> stations = await _context.Stations.Include(p => p.Synonym)
                                          .Include(p => p.StationInfo)
                                          .Include(x => x.StationInfoTranslation)
                                          .Include(y => y.StationGate)
                                                .ThenInclude(a => a.StationGateServices)
                                                .ThenInclude(b => b.StationServices)

                                                    .Where(p => p.IsActive)
                                                    .ToArrayAsync().ConfigureAwait(false);

        return stations;
    }
    private async Task<IEnumerable<Station>> GetCacheStationsAsync()
    {
        IEnumerable<Station> stations = await _cacheService.GetAsync<IEnumerable<Station>>(CacheKeys.Stations).ConfigureAwait(false);

        if (stations == null || stations.Count() <= 0)
        {
            stations = await GetStationsNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<Station>>(CacheKeys.Stations, stations).ConfigureAwait(false);

        }

        return stations;
    }
    public async Task<IEnumerable<StationInfo>> GetStationsInfoAsync()
    {
        IEnumerable<StationInfo> stationInfo = await GetStationsInfoNoCache().ConfigureAwait(false);

        return stationInfo;
    }

    private async Task<IEnumerable<StationInfo>> GetStationsInfoNoCache()
    {
        IEnumerable<StationInfo> stationInfo = await _context.StationInfo.ToArrayAsync().ConfigureAwait(false);

       
        return stationInfo;
    }

    public async Task<StationInfo> GetStationInfoAsync(int id)
    {
        IEnumerable<StationInfo> stationsInfo = await GetStationsInfoNoCache().ConfigureAwait(false);
        StationInfo stationInfo = stationsInfo.SingleOrDefault(p => p.StationId == id);
        return stationInfo;
    }

    public async Task<IEnumerable<ParkingCosts>> GetParkingCostsAsync()
    {
        IEnumerable<ParkingCosts> parkingCosts = await _context.ParkingCosts.ToArrayAsync().ConfigureAwait(false);
        return parkingCosts;
    }
    public async Task<StationInformationRsponseDto> GetStationInformationAsync(StationInformationRequestDto request)
    {
        List<Translation> translations = await _context.Translations.Where(x => x.SystemTypeId == request.SystemType).ToListAsync();
        IEnumerable<StationInfoTranslation> stationInfoTranslation = await _context.StationInfoTranslation.ToListAsync().ConfigureAwait(false);
        StationInformationRsponseDto stationInformationRsponseDto = new();
        try
        {
                                  stationInformationRsponseDto.StationUpdates = await _railUpdatesService.GetRailGeneralUpdatesUmbracoByStationIdAsync(request.LanguageId, request.StationId);
            stationInformationRsponseDto.StationDetails = await GetStationDetailsAsync(request.StationId, request.LanguageId, request.SystemType, translations);
            stationInformationRsponseDto.GateInfo = await GateInfoAsync(request.StationId, request.LanguageId, translations, stationInfoTranslation);
            stationInformationRsponseDto.EasyCategories = new List<string> {
                                                                            "Restaurants-and-Fast-Food", "Shopping", "Events",
                                                                            "Activities-&-Recreation", "Tourism-&-Travel",
                                                                            "Help-&-Services", "All-Institutes"
                                                                        };

            stationInformationRsponseDto.SafetyInfos = await GetSafetyInfos(request.LanguageId, translations);
            return stationInformationRsponseDto;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<List<string>> GetSafetyInfos(Languages languageId, List<Translation> translations)
    {
        List<string> safelyInfos = new();
        switch (languageId)
        {
            case Languages.Hebrew:
                safelyInfos = translations.Where(x => x.Key.Contains("SafetyInfo")).Select(x => x.Hebrew).ToList();
                break;
            case Languages.English:
                safelyInfos = translations.Where(x => x.Key.Contains("SafetyInfo")).Select(x => x.English).ToList();
                break;
            case Languages.Arabic:
                safelyInfos = translations.Where(x => x.Key.Contains("SafetyInfo")).Select(x => x.Arabic).ToList();
                break;
            case Languages.Russian:
                safelyInfos = translations.Where(x => x.Key.Contains("SafetyInfo")).Select(x => x.Russian).ToList();
                break;
        }
        return safelyInfos;
    }

    private async Task<List<GateInfo>> GateInfoAsync(int StationId, Languages languageId, IEnumerable<Translation> translations, IEnumerable<StationInfoTranslation> stationInfoTranslation)
    {
        List<GateInfo> GateInfo = new();
        List<StationGate> StationGates = await _context.StationGate.Where(x => x.StationId == StationId).ToListAsync().ConfigureAwait(false);

        var ActivityHours = await _context.StationGateActivityHoursLines.Join(_context.StationGateActivityHours,
                                                                                   q => q.StationHoursId,
                                                                                   stationGateActivityHours => stationGateActivityHours.StationHoursId,
                                                                                   (stationGateActivityHours, q) => new { stationGateActivityHours, q }).ToListAsync().ConfigureAwait(false);
        List<StationGateServices> StationGateServices = await _context.StationGateServices.ToListAsync().ConfigureAwait(false);
        List<StationServices> StationServices = await _context.StationServices.ToListAsync().ConfigureAwait(false);

        switch (languageId)
        {
            case Languages.Hebrew:
                var cultureInfo = new CultureInfo("he-IL");
                GateInfo = StationGates.Select(x => new GateInfo
                {
                    StationGateId = x.StationGateId,
                    GateAddress = stationInfoTranslation.Where(y => y.Key == x.GateAddressTranslationKey).Select(y => y.Hebrew).FirstOrDefault(),
                    GateLatitude = x.GateLatitude,
                    GateLontitude = x.GateLontitude,
                    GateName = stationInfoTranslation.Where(y => y.Key == x.GateNameTranslationKey).Select(y => y.Hebrew).FirstOrDefault(),
                    GateActivityHours = ActivityHours.Where(e => e.q.StationGateId == x.StationGateId).Select(e => new GateActivityHours
                    {
                        ActivityHoursType = e.q.TemplateTypeId,                        IsClosedShortText = (
                                            e.q.IsClosed
                                            && (e.q.ClosedUntill > DateTime.Now || e.q.ClosedUntill == new DateTime(0001, 01, 01))
                                            )                                          ? translations.Where(z => z.Key == ((e.q.ClosedUntill != null && e.q.ClosedUntill > DateTime.Now)                                                                                              ? "StationGateorCashierIsTemporaryClosed"
                                                                                              : "StationGateorCashierIsClosed"))
                                                                                               .Select(z => z.Hebrew).FirstOrDefault()
                                                                                                : "",

                        IsClosedLongText = (e.q.IsClosed && e.q.ClosedUntill != null && e.q.ClosedUntill > DateTime.Now)
                                                                                         ? translations.Where(z => z.Key == (
                                                                                             (e.q.TemplateTypeId == 1) ? "StationGateIsClosedUntill"
                                                                                                                     : ((e.q.TemplateTypeId == 2) ? "StationGateorCashierIsClosedUntill"
                                                                                                                                                 : /*q.TemplateTypeId == 3?*/  "StationGateRavKavIsClosedUntill")))
                                                                                                                                                .Select(z => z.Hebrew).FirstOrDefault() + e.q.ClosedUntill.Value.ToString("ddd dd/MM/yy", cultureInfo)
                                                                                         : "",

                        ActivityDaysNumbers = e.stationGateActivityHours.ActivityDaysNumbers,
                        StartHourTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.StartHourPrefixTextKey)
                                                                                                                                  .Select(z => z.Hebrew).FirstOrDefault(),
                        StartHour = e.stationGateActivityHours.StartHour.ToString(@"hh\:mm"),
                        StartHourReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.StartHourReplaceTextKey)
                                                                                                                                  .Select(z => z.Hebrew).FirstOrDefault(),
                        EndHourPrefixTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourPrefixTextKey)
                                                                                                                                  .Select(z => z.Hebrew).FirstOrDefault(),
                        EndHour = e.stationGateActivityHours.EndHour.ToString(@"hh\:mm"),
                        EndHourReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourReplaceTextKey)
                                                                                                                                  .Select(z => z.Hebrew).FirstOrDefault(),
                        EndHourPostfixTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourPostfixTextKey)
                                                                                                                                  .Select(z => z.Hebrew).FirstOrDefault(),
                        ActivityHoursReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.ActivityHoursReplaceTextKey)
                                                                                                                                  .Select(z => z.Hebrew).FirstOrDefault(),

                    })

                    .ToList(),
                    GateServices = StationGateServices
                               .Where(y => y.StationGateId == x.StationGateId && y.isServiceExist)
                              .Join(StationServices,
                                                           x => x.ServiceId,
                                                           y => y.ServiceId,
                                                           (x, y) =>
                                                           new GateServices
                                                           {
                                                               ServiceName = y.Hebrew,
                                                               ServiceIcon = y.IconId,
                                                               ServiceIconLink = y.IconLink
                                                           }).ToList()


                }).ToList();
                break;

            case Languages.English:
                var cultureInfoEn = new CultureInfo("en-Us");
                GateInfo = StationGates.Where(x => x.StationId == StationId).Select(x => new GateInfo
                {
                    StationGateId = x.StationGateId,
                    GateAddress = stationInfoTranslation.Where(y => y.Key == x.GateAddressTranslationKey).Select(y => y.English).FirstOrDefault(),
                    GateLatitude = x.GateLatitude,
                    GateLontitude = x.GateLontitude,
                    GateName = stationInfoTranslation.Where(y => y.Key == x.GateNameTranslationKey).Select(y => y.English).FirstOrDefault(),
                    GateActivityHours = ActivityHours.Where(e => e.q.StationGateId == x.StationGateId).Select(e => new GateActivityHours
                    {
                        ActivityHoursType = e.q.TemplateTypeId,                        IsClosedShortText = (
                                            e.q.IsClosed
                                            && (e.q.ClosedUntill > DateTime.Now || e.q.ClosedUntill == new DateTime(0001, 01, 01))
                                            )                                                                                                ? translations.Where(z => z.Key == ((e.q.ClosedUntill != null && e.q.ClosedUntill > DateTime.Now)                                                                                                                                                    ? "StationGateorCashierIsTemporaryClosed"
                                                                                                                                                    : "StationGateorCashierIsClosed"))
                                                                                                                                                            .Select(z => z.English).FirstOrDefault()
                                                                                                : "",

                        IsClosedLongText = (e.q.IsClosed && e.q.ClosedUntill != null && e.q.ClosedUntill > DateTime.Now)
                                                                                         ? translations.Where(z => z.Key == (
                                                                                             (e.q.TemplateTypeId == 1) ? "StationGateIsClosedUntill"
                                                                                                                     : ((e.q.TemplateTypeId == 2) ? "StationGateorCashierIsClosedUntill"
                                                                                                                                                 : /*e.q.TemplateTypeId == 3?*/  "StationGateRavKavIsClosedUntill")))
                                                                                                                                                .Select(z => z.English).FirstOrDefault() + e.q.ClosedUntill.Value.ToString("ddd dd/MM/yy", cultureInfoEn)
                                                                                         : "",

                        ActivityDaysNumbers = e.stationGateActivityHours.ActivityDaysNumbers,
                        StartHourTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.StartHourPrefixTextKey)
                                                                                                                                  .Select(z => z.English).FirstOrDefault(),
                        StartHour = e.stationGateActivityHours.StartHour.ToString(@"hh\:mm"),
                        StartHourReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.StartHourReplaceTextKey)
                                                                                                                                  .Select(z => z.English).FirstOrDefault(),
                        EndHourPrefixTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourPrefixTextKey)
                                                                                                                                  .Select(z => z.English).FirstOrDefault(),
                        EndHour = e.stationGateActivityHours.EndHour.ToString(@"hh\:mm"),
                        EndHourReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourReplaceTextKey)
                                                                                                                                  .Select(z => z.English).FirstOrDefault(),
                        EndHourPostfixTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourPostfixTextKey)
                                                                                                                                  .Select(z => z.English).FirstOrDefault(),
                        ActivityHoursReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.ActivityHoursReplaceTextKey)
                                                                                                                                  .Select(z => z.English).FirstOrDefault(),

                    }).ToList(),
                    GateServices = StationGateServices
                 .Where(y => y.StationGateId == x.StationGateId && y.isServiceExist)
                  .Join(StationServices,
                                                           x => x.ServiceId,
                                                           y => y.ServiceId,
                                                           (x, y) =>
                                                           new GateServices
                                                           {
                                                               ServiceName = y.English,
                                                               ServiceIcon = y.IconId,
                                                               ServiceIconLink = y.IconLink
                                                           }).ToList()
                }).ToList();
                break;
            case Languages.Arabic:
                var cultureInfoAr = new CultureInfo("ar");
                GateInfo = StationGates.Where(x => x.StationId == StationId).Select(x => new GateInfo
                {
                    StationGateId = x.StationGateId,
                    GateAddress = stationInfoTranslation.Where(y => y.Key == x.GateAddressTranslationKey).Select(y => y.Arabic).FirstOrDefault(),
                    GateLatitude = x.GateLatitude,
                    GateLontitude = x.GateLontitude,
                    GateName = stationInfoTranslation.Where(y => y.Key == x.GateNameTranslationKey).Select(y => y.Arabic).FirstOrDefault(),
                    GateActivityHours = ActivityHours.Where(e => e.q.StationGateId == x.StationGateId).Select(e => new GateActivityHours
                    {
                        ActivityHoursType = e.q.TemplateTypeId,                        IsClosedShortText = (
                                            e.q.IsClosed
                                            && (e.q.ClosedUntill > DateTime.Now || e.q.ClosedUntill == new DateTime(0001, 01, 01))
                                            )                                                                                                ? translations.Where(z => z.Key == ((e.q.ClosedUntill != null && e.q.ClosedUntill > DateTime.Now)                                                                                                                                                    ? "StationGateorCashierIsTemporaryClosed"
                                                                                                                                                    : "StationGateorCashierIsClosed"))
                                                                                                                                                            .Select(z => z.Arabic).FirstOrDefault()
                                                                                                : "",

                        IsClosedLongText = (e.q.IsClosed && e.q.ClosedUntill != null && e.q.ClosedUntill > DateTime.Now)
                                                                                         ? translations.Where(z => z.Key == (
                                                                                             (e.q.TemplateTypeId == 1) ? "StationGateIsClosedUntill"
                                                                                                                     : ((e.q.TemplateTypeId == 2) ? "StationGateorCashierIsClosedUntill"
                                                                                                                                                 : /*e.q.TemplateTypeId == 3?*/  "StationGateRavKavIsClosedUntill")))
                                                                                                                                                .Select(z => z.Arabic).FirstOrDefault() + e.q.ClosedUntill.Value.ToString("ddd ", cultureInfoAr) + e.q.ClosedUntill.Value.ToString("dd/MM/yy")
                                                                                         : "",

                        ActivityDaysNumbers = e.stationGateActivityHours.ActivityDaysNumbers,
                        StartHourTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.StartHourPrefixTextKey)
                                                                                                                                  .Select(z => z.Arabic).FirstOrDefault(),
                        StartHour = e.stationGateActivityHours.StartHour.ToString(@"hh\:mm"),
                        StartHourReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.StartHourReplaceTextKey)
                                                                                                                                  .Select(z => z.Arabic).FirstOrDefault(),
                        EndHourPrefixTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourPrefixTextKey)
                                                                                                                                  .Select(z => z.Arabic).FirstOrDefault(),
                        EndHour = e.stationGateActivityHours.EndHour.ToString(@"hh\:mm"),
                        EndHourReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourReplaceTextKey)
                                                                                                                                  .Select(z => z.Arabic).FirstOrDefault(),
                        EndHourPostfixTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourPostfixTextKey)
                                                                                                                                  .Select(z => z.Arabic).FirstOrDefault(),
                        ActivityHoursReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.ActivityHoursReplaceTextKey)
                                                                                                                                  .Select(z => z.Arabic).FirstOrDefault(),

                    }).ToList(),
                    GateServices = StationGateServices
                             .Where(y => y.StationGateId == x.StationGateId && y.isServiceExist)
                              .Join(StationServices,
                                                           x => x.ServiceId,
                                                           y => y.ServiceId,
                                                           (x, y) =>
                                                           new GateServices
                                                           {
                                                               ServiceName = y.Arabic,
                                                               ServiceIcon = y.IconId,
                                                               ServiceIconLink = y.IconLink
                                                           }).ToList()


                }).ToList();
                break;
            case Languages.Russian:
                var cultureInfoRu = new CultureInfo("ru-RU");
                GateInfo = StationGates.Where(x => x.StationId == StationId).Select(x => new GateInfo
                {
                    StationGateId = x.StationGateId,
                    GateAddress = stationInfoTranslation.Where(y => y.Key == x.GateAddressTranslationKey).Select(y => y.Russian).FirstOrDefault(),
                    GateLatitude = x.GateLatitude,
                    GateLontitude = x.GateLontitude,
                    GateName = stationInfoTranslation.Where(y => y.Key == x.GateNameTranslationKey).Select(y => y.Russian).FirstOrDefault(),
                    GateActivityHours = ActivityHours.Where(e => e.q.StationGateId == x.StationGateId).Select(e => new GateActivityHours
                    {
                        ActivityHoursType = e.q.TemplateTypeId,                        IsClosedShortText = (
                                            e.q.IsClosed
                                            && (e.q.ClosedUntill > DateTime.Now || e.q.ClosedUntill == new DateTime(0001, 01, 01))
                                            )                                                                                                ? translations.Where(z => z.Key == ((e.q.ClosedUntill != null && e.q.ClosedUntill > DateTime.Now)                                                                                                                                                    ? "StationGateorCashierIsTemporaryClosed"
                                                                                                                                                    : "StationGateorCashierIsClosed"))
                                                                                                                                                            .Select(z => z.Russian).FirstOrDefault()
                                                                                                : "",

                        IsClosedLongText = (e.q.IsClosed && e.q.ClosedUntill != null && e.q.ClosedUntill > DateTime.Now)
                                                                                         ? translations.Where(z => z.Key == (
                                                                                             (e.q.TemplateTypeId == 1) ? "StationGateIsClosedUntill"
                                                                                                                     : ((e.q.TemplateTypeId == 2) ? "StationGateorCashierIsClosedUntill"
                                                                                                                                                 : /*e.q.TemplateTypeId == 3?*/  "StationGateRavKavIsClosedUntill")))
                                                                                                                                                .Select(z => z.Russian).FirstOrDefault() + e.q.ClosedUntill.Value.ToString("ddd dd/MM/yy", cultureInfoRu)
                                                                                         : "",

                        ActivityDaysNumbers = e.stationGateActivityHours.ActivityDaysNumbers,
                        StartHourTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.StartHourPrefixTextKey)
                                                                                                                                  .Select(z => z.Russian).FirstOrDefault(),
                        StartHour = e.stationGateActivityHours.StartHour.ToString(@"hh\:mm"),
                        StartHourReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.StartHourReplaceTextKey)
                                                                                                                                  .Select(z => z.Russian).FirstOrDefault(),
                        EndHourPrefixTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourPrefixTextKey)
                                                                                                                                  .Select(z => z.Russian).FirstOrDefault(),
                        EndHour = e.stationGateActivityHours.EndHour.ToString(@"hh\:mm"),
                        EndHourReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourReplaceTextKey)
                                                                                                                                  .Select(z => z.Russian).FirstOrDefault(),
                        EndHourPostfixTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.EndHourPostfixTextKey)
                                                                                                                                  .Select(z => z.Russian).FirstOrDefault(),
                        ActivityHoursReplaceTextKey = translations.Where(z => z.Key == e.stationGateActivityHours.ActivityHoursReplaceTextKey)
                                                                                                                                  .Select(z => z.Russian).FirstOrDefault(),

                    }).ToList(),
                    GateServices = StationGateServices
                 .Where(y => y.StationGateId == x.StationGateId && y.isServiceExist)
                  .Join(StationServices,
                                                           x => x.ServiceId,
                                                           y => y.ServiceId,
                                                           (x, y) =>
                                                           new GateServices
                                                           {
                                                               ServiceName = y.Russian,
                                                               ServiceIcon = y.IconId,
                                                               ServiceIconLink = y.IconLink
                                                           }).ToList()


                }).ToList();
                break;
        }

        return GateInfo;
    }

    private async Task<IEnumerable<StationUpdate>> GetRailUpdatesForStationAsync(Languages languageId, int stationId, string url, string contentType)
    {
        string json = await _httpClient.GetRailInfoAsync(url, contentType).ConfigureAwait(false);

        RailUpdate railUpdates = JsonConvert.DeserializeObject<RailUpdate>(json);
        List<StationUpdate> result = new();
        foreach (UpdateStationNode update in railUpdates.Data)
        {
            if (update.Stations.Count() > 0)
            {
                var temp = Array.IndexOf(update.Stations.ToArray(), stationId.ToString());
                if (temp > -1)
                {
                    StationUpdate stationUpdate = null;
                    switch (languageId)
                    {
                        case Languages.Hebrew:
                            stationUpdate = new StationUpdate(update.NameHeb, update.UpdateLinkHeb);
                            break;
                        case Languages.English:
                            stationUpdate = new StationUpdate(update.NameEng, update.UpdateLinkEng);
                            break;
                        case Languages.Russian:
                            stationUpdate = new StationUpdate(update.NameRus, update.UpdateLinkRus);
                            break;
                        case Languages.Arabic:
                            stationUpdate = new StationUpdate(update.NameArb, update.UpdateLinkArb);
                            break;

                    }
                    result.Add(stationUpdate);
                }
            }
        }

        return result;
    }

    private async Task<StationDetails> GetStationDetailsAsync(int stationId, Languages languageId, int systemType, List<Translation> translations)
    {
        StationDetails stationDetails = new();
        stationDetails.stationId = stationId;
        Station station = await GetStationAsync(stationId);
        List<String> gates = new();

        if (translations == null)
        {
            throw new Exception("Translations cannot be null", null);
        }


        if (station.Parking == true)
        {
            gates = await _context.StationGate.Where(x => x.StationId == stationId && x.GateParking != null)
                .Select(x => x.GateNameTranslationKey).ToListAsync().ConfigureAwait(false);
        }

        var colInfo = translations.GetType().GetProperties();
        Translation translationCarParking = new();
        string gatesNames = "";

        switch (gates.Count)
        {
            case 1:
                translationCarParking = translations
                   .Where(x => x.Key == "StationCarParkingLot" && x.SystemTypeId == systemType).FirstOrDefault();
                break;
            case 0:
                translationCarParking = translations
                    .Where(x => x.Key == "StationNoCarParkingLot" && x.SystemTypeId == systemType).FirstOrDefault();
                break;
            case > 1:
                translationCarParking = translations
                    .Where(x => x.Key == "StationCarParkingLots" && x.SystemTypeId == systemType).FirstOrDefault();
                               break;

        }
        List<ParkingCosts> parkingCosts = _context.ParkingCosts.ToList();
        StationInfo stationInfo = await _context.StationInfo.Where(x => x.StationId == stationId).FirstOrDefaultAsync().ConfigureAwait(false);
        InfoDto costs =
               new()
               {
                   ParkCosts = parkingCosts.Where(y => y.Id == stationInfo.ParkingCosts).Select(y => y.Key).FirstOrDefault(),
                   BikeParkCosts = parkingCosts.Where(y => y.Id == stationInfo.BikeParkingCosts).Select(y => y.Key).FirstOrDefault(),
                   BikeParking = stationInfo.BikeParking,
                   AirPollution = stationInfo.AirPolution,
                   StationMap = stationInfo.StationMap,
                   NonActiveElevators = stationInfo.NonActiveElavators == null ? string.Empty : stationInfo.NonActiveElavators,
                   StationIsClosed = stationInfo.StationIsClosed,
                   StationIsClosedUntill = stationInfo.StationIsClosedUntill
               };

        Translation translationBikeParking = null;

        if (costs.AirPollution)
        {
            if (systemType == (int)SystemTypes.Web)
                stationDetails.AirPollutionIcon = _context.ConfigurationParameter.Where(x => x.Key == "AirPollutionIcon").Select(x => x.ValueWeb).FirstOrDefault();
            else
                stationDetails.AirPollutionIcon = _context.ConfigurationParameter.Where(x => x.Key == "AirPollutionIcon").Select(x => x.ValueMob).FirstOrDefault();
        }

        stationDetails.StationMap = costs.StationMap;

        switch (costs.BikeParking)
        {
            case true:
                translationBikeParking = translations
                   .Where(x => x.Key == "StationBikesParkingLot" && x.SystemTypeId == systemType).FirstOrDefault();
                break;


        }
        stationDetails.StationIsClosed = costs.StationIsClosed == true;        stationDetails.StationIsClosedUntill = costs.StationIsClosedUntill;
        Translation translationStationIsClose = new();
        if (stationDetails.StationIsClosed == true)
        {
            switch (costs.StationIsClosedUntill.ToString().Length)
            {
                case 0:
                    translationStationIsClose = translations.Where(x => x.Key == "StationIsClosed").FirstOrDefault();
                    break;
                case > 0:
                    translationStationIsClose = translations.Where(x => x.Key == "StationIsClosedUntill").FirstOrDefault();
                    break;
            }
        }



        var gatTranslation = await _context.StationInfoTranslation.Where(t => gates.Any(g => g == t.Key)).ToListAsync().ConfigureAwait(false);
        switch (languageId)
        {
            case Languages.Hebrew:
                var cultureInfo = new CultureInfo("he-IL");
                if (gatTranslation.Select(x => x.Hebrew).ToArray().Length > 1)
                    gatesNames = string.Join(", ", gatTranslation.Select(x => x.Hebrew).ToArray());
                stationDetails.CarParking = translationCarParking != null ? translationCarParking.Hebrew != null ? translationCarParking.Hebrew + gatesNames : "" : "";
                stationDetails.ParkingCosts = translations.Where(x => x.Key == costs.ParkCosts).Select(x => x.Hebrew).FirstOrDefault();
                stationDetails.BikeParkingCosts = translations.Where(x => x.Key == costs.BikeParkCosts).Select(x => x.Hebrew).FirstOrDefault();
                stationDetails.BikeParking = translationBikeParking != null ? translationBikeParking.Hebrew : "";
                if (costs.NonActiveElevators.Length > 0)
                {
                    stationDetails.NonActiveElevators = string.Format(translations.Where(x => x.Key == "NonActiveElevatorsToPlatforms").Select(x => x.Hebrew).FirstOrDefault(), costs.NonActiveElevators);
                    if (systemType == (int)SystemTypes.Web)
                        stationDetails.NonActiveElevatorsLink = _context.ConfigurationParameter.Where(x => x.Key == "TrainAccessibilityPageHe").Select(x => x.ValueWeb).FirstOrDefault();
                    else
                        stationDetails.NonActiveElevatorsLink = _context.ConfigurationParameter.Where(x => x.Key == "TrainAccessibilityPageHe").Select(x => x.ValueMob).FirstOrDefault();
                }


                if (stationDetails.StationIsClosed == true)
                {
                    if (costs.StationIsClosedUntill == new DateTime(0001, 01, 01))
                    {
                        stationDetails.StationIsClosedText = translations.Where(x => x.Key == "StationIsClosed").Select(x => x.Hebrew).FirstOrDefault();
                    }
                    else if (costs.StationIsClosedUntill <= DateTime.Now)
                    {
                        stationDetails.StationIsClosed = false;
                        stationDetails.StationIsClosedText = "";
                        costs.StationIsClosed = false;
                    }
                    else
                    {
                        stationDetails.StationIsClosedText = string.Format(translations.Where(x => x.Key == "StationIsClosedUntill").Select(x => x.Hebrew).FirstOrDefault(), costs.StationIsClosedUntill.Value.ToString("ddd dd/MM/yy", cultureInfo));
                    }
                }
                DateTime date = DateTime.Now;
                if (stationInfo.StationInfoFromDate < date && stationInfo.StationInfoToDate > date)
                {
                    stationDetails.StationInfoTitle = _context.StationInfoTranslation.Where(x => x.Key == "StationInfoTitle" && x.StationId == stationId).Select(x => x.Hebrew).FirstOrDefault();
                    stationDetails.StationInfo = _context.StationInfoTranslation.Where(x => x.Key == "StationInfoContent" && x.StationId == stationId).Select(x => x.Hebrew).FirstOrDefault();
                }

                stationDetails.AboutStationTitle = _context.StationInfoTranslation.Where(x => x.Key == "AboutTitle" && x.StationId == stationId).Select(x => x.Hebrew).FirstOrDefault();
                stationDetails.AboutStationContent = _context.StationInfoTranslation.Where(x => x.Key == "AboutContent" && x.StationId == stationId).Select(x => x.Hebrew).FirstOrDefault();
                stationDetails.stationName = station.HebrewName;
                stationDetails.ParkingTitleTranslationKey= _context.StationInfoTranslation.Where(x => x.Key == "ParkingTitleTranslationKey" && x.StationId == stationId).Select(x => x.Hebrew).FirstOrDefault();
                stationDetails.ParkingContentTranslationKey = _context.StationInfoTranslation.Where(x => x.Key == "ParkingContentTranslationKey" && x.StationId == stationId).Select(x => x.Hebrew).FirstOrDefault();
                break;
            case Languages.English:
                var cultureInfoEn = new CultureInfo("en-Us");
                if (gatTranslation.Select(x => x.English).ToArray().Length > 1)
                    gatesNames = string.Join(", ", gatTranslation.Select(x => x.English).ToArray());
                stationDetails.CarParking = translationCarParking != null ? translationCarParking.English != null ? translationCarParking.English + gatesNames : "" : "";
                stationDetails.ParkingCosts = translations.Where(x => x.Key == costs.ParkCosts).Select(x => x.English).FirstOrDefault();
                stationDetails.BikeParkingCosts = translations.Where(x => x.Key == costs.BikeParkCosts).Select(x => x.English).FirstOrDefault();
                stationDetails.BikeParking = translationBikeParking != null ? translationBikeParking.English : "";
                if (costs.NonActiveElevators.Length > 0)
                {
                    stationDetails.NonActiveElevators = string.Format(translations.Where(x => x.Key == "NonActiveElevatorsToPlatforms").Select(x => x.English).FirstOrDefault(), costs.NonActiveElevators);
                    if (systemType == (int)SystemTypes.Web)
                        stationDetails.NonActiveElevatorsLink = _context.ConfigurationParameter.Where(x => x.Key == "TrainAccessibilityPageEn").Select(x => x.ValueWeb).FirstOrDefault();
                    else
                        stationDetails.NonActiveElevatorsLink = _context.ConfigurationParameter.Where(x => x.Key == "TrainAccessibilityPageEn").Select(x => x.ValueMob).FirstOrDefault();
                }
                if (stationDetails.StationIsClosed == true)
                {
                    if (costs.StationIsClosedUntill == new DateTime(0001, 01, 01))
                    {

                        stationDetails.StationIsClosedText = translations.Where(x => x.Key == "StationIsClosed").Select(x => x.English).FirstOrDefault();
                    }
                    else if (costs.StationIsClosedUntill <= DateTime.Now)
                    {
                        stationDetails.StationIsClosed = false;
                        stationDetails.StationIsClosedText = "";
                        costs.StationIsClosed = false;
                    }
                    else
                    {
                        stationDetails.StationIsClosedText = string.Format(translations.Where(x => x.Key == "StationIsClosedUntill").Select(x => x.English).FirstOrDefault(), costs.StationIsClosedUntill.Value.ToString("ddd MM/dd/yy", cultureInfoEn));
                    }
                }
                stationDetails.StationInfoTitle = _context.StationInfoTranslation.Where(x => x.Key == "StationInfoTitle" && x.StationId == stationId).Select(x => x.English).FirstOrDefault();
                stationDetails.StationInfo = _context.StationInfoTranslation.Where(x => x.Key == "StationInfoContent" && x.StationId == stationId).Select(x => x.English).FirstOrDefault();
                stationDetails.AboutStationTitle = _context.StationInfoTranslation.Where(x => x.Key == "AboutTitle" && x.StationId == stationId).Select(x => x.English).FirstOrDefault();
                stationDetails.AboutStationContent = _context.StationInfoTranslation.Where(x => x.Key == "AboutContent" && x.StationId == stationId).Select(x => x.English).FirstOrDefault();
                stationDetails.stationName = station.EnglishName;
                stationDetails.ParkingTitleTranslationKey = _context.StationInfoTranslation.Where(x => x.Key =="ParkingTitleTranslationKey" && x.StationId == stationId).Select(x => x.English).FirstOrDefault();
                stationDetails.ParkingContentTranslationKey = _context.StationInfoTranslation.Where(x => x.Key == "ParkingContentTranslationKey" && x.StationId == stationId).Select(x => x.English).FirstOrDefault();

                break;
            case Languages.Arabic:
                var cultureInfoAr = new CultureInfo("ar");
                if (gatTranslation.Select(x => x.Arabic).ToArray().Length > 1)
                    gatesNames = string.Join(", ", gatTranslation.Select(x => x.Arabic).ToArray());
                stationDetails.CarParking = translationCarParking != null ? translationCarParking.Arabic != null ? translationCarParking.Arabic + gatesNames : "" : "";
                stationDetails.ParkingCosts = translations.Where(x => x.Key == costs.ParkCosts).Select(x => x.Arabic).FirstOrDefault();
                stationDetails.BikeParkingCosts = translations.Where(x => x.Key == costs.BikeParkCosts).Select(x => x.Arabic).FirstOrDefault();
                stationDetails.BikeParking = translationBikeParking != null ? translationBikeParking.Arabic : "";
                if (costs.NonActiveElevators.Length > 0)
                {
                    stationDetails.NonActiveElevators = string.Format(translations.Where(x => x.Key == "NonActiveElevatorsToPlatforms").Select(x => x.Arabic).FirstOrDefault(), costs.NonActiveElevators);

                    if (systemType == (int)SystemTypes.Web)
                        stationDetails.NonActiveElevatorsLink = _context.ConfigurationParameter.Where(x => x.Key == "TrainAccessibilityPageAr").Select(x => x.ValueWeb).FirstOrDefault();
                    else
                        stationDetails.NonActiveElevatorsLink = _context.ConfigurationParameter.Where(x => x.Key == "TrainAccessibilityPageAr").Select(x => x.ValueMob).FirstOrDefault();
                }

                if (stationDetails.StationIsClosed == true)
                {
                    if (costs.StationIsClosedUntill == new DateTime(0001, 01, 01))
                    {

                        stationDetails.StationIsClosedText = translations.Where(x => x.Key == "StationIsClosed").Select(x => x.Arabic).FirstOrDefault();
                    }
                    else if (costs.StationIsClosedUntill <= DateTime.Now)
                    {
                        stationDetails.StationIsClosed = false;
                        stationDetails.StationIsClosedText = "";
                        costs.StationIsClosed = false;
                    }
                    else
                    {
                        stationDetails.StationIsClosedText = string.Format(translations.Where(x => x.Key == "StationIsClosedUntill").Select(x => x.Arabic).FirstOrDefault(), costs.StationIsClosedUntill.Value.ToString("ddd ", cultureInfoAr) + costs.StationIsClosedUntill.Value.ToString("dd/MM/yy"));
                    }
                }

                stationDetails.StationInfoTitle = _context.StationInfoTranslation.Where(x => x.Key == "StationInfoTitle" && x.StationId == stationId).Select(x => x.Arabic).FirstOrDefault();
                stationDetails.StationInfo = _context.StationInfoTranslation.Where(x => x.Key == "StationInfoContent" && x.StationId == stationId).Select(x => x.Arabic).FirstOrDefault();
                stationDetails.AboutStationTitle = _context.StationInfoTranslation.Where(x => x.Key == "AboutTitle" && x.StationId == stationId).Select(x => x.Arabic).FirstOrDefault();
                stationDetails.AboutStationContent = _context.StationInfoTranslation.Where(x => x.Key == "AboutContent" && x.StationId == stationId).Select(x => x.Arabic).FirstOrDefault();
                stationDetails.stationName = station.ArabicName;
                stationDetails.ParkingTitleTranslationKey = _context.StationInfoTranslation.Where(x => x.Key =="ParkingTitleTranslationKey" && x.StationId == stationId).Select(x => x.Arabic).FirstOrDefault();
                stationDetails.ParkingContentTranslationKey = _context.StationInfoTranslation.Where(x => x.Key == "ParkingContentTranslationKey" && x.StationId == stationId).Select(x => x.Arabic).FirstOrDefault();

                break;
            case Languages.Russian:
                var cultureInfoRu = new CultureInfo("ru-RU");
                if (gatTranslation.Select(x => x.Russian).ToArray().Length > 1)
                    gatesNames = string.Join(", ", gatTranslation.Select(x => x.Russian).ToArray());
                stationDetails.CarParking = translationCarParking != null ? translationCarParking.Russian != null ? translationCarParking.Russian + gatesNames : "" : "";
                stationDetails.ParkingCosts = translations.Where(x => x.Key == costs.ParkCosts).Select(x => x.Russian).FirstOrDefault();
                stationDetails.BikeParkingCosts = translations.Where(x => x.Key == costs.BikeParkCosts).Select(x => x.Russian).FirstOrDefault();
                stationDetails.BikeParking = translationBikeParking != null ? translationBikeParking.Russian : "";

                if (costs.NonActiveElevators.Length > 0)
                {
                    stationDetails.NonActiveElevators = string.Format(translations.Where(x => x.Key == "NonActiveElevatorsToPlatforms").Select(x => x.Russian).FirstOrDefault(), costs.NonActiveElevators);

                    if (systemType == (int)SystemTypes.Web)
                        stationDetails.NonActiveElevatorsLink = _context.ConfigurationParameter.Where(x => x.Key == "TrainAccessibilityPageRu").Select(x => x.ValueWeb).FirstOrDefault();
                    else
                        stationDetails.NonActiveElevatorsLink = _context.ConfigurationParameter.Where(x => x.Key == "TrainAccessibilityPageRu").Select(x => x.ValueMob).FirstOrDefault();
                }
                if (stationDetails.StationIsClosed == true)
                {
                    if (costs.StationIsClosedUntill == new DateTime(0001, 01, 01))
                    {

                        stationDetails.StationIsClosedText = translations.Where(x => x.Key == "StationIsClosed").Select(x => x.Russian).FirstOrDefault();
                    }
                    else if (costs.StationIsClosedUntill <= DateTime.Now)
                    {
                        stationDetails.StationIsClosed = false;
                        stationDetails.StationIsClosedText = "";
                        costs.StationIsClosed = false;
                    }
                    else
                    {
                        stationDetails.StationIsClosedText = string.Format(translations.Where(x => x.Key == "StationIsClosedUntill").Select(x => x.Russian).FirstOrDefault(), costs.StationIsClosedUntill.Value.ToString("ddd dd/MM/yy", cultureInfoRu));
                    }
                }

                stationDetails.StationInfoTitle = _context.StationInfoTranslation.Where(x => x.Key == "StationInfoTitle" && x.StationId == stationId).Select(x => x.Russian).FirstOrDefault();
                stationDetails.StationInfo = _context.StationInfoTranslation.Where(x => x.Key == "StationInfoContent" && x.StationId == stationId).Select(x => x.Russian).FirstOrDefault();
                stationDetails.AboutStationTitle = _context.StationInfoTranslation.Where(x => x.Key == "AboutTitle" && x.StationId == stationId).Select(x => x.Russian).FirstOrDefault();
                stationDetails.AboutStationContent = _context.StationInfoTranslation.Where(x => x.Key == "AboutContent" && x.StationId == stationId).Select(x => x.Russian).FirstOrDefault();
                stationDetails.stationName = station.RussianName;
                stationDetails.ParkingTitleTranslationKey = _context.StationInfoTranslation.Where(x => x.Key =="ParkingTitleTranslationKey" && x.StationId == stationId).Select(x => x.Russian).FirstOrDefault();
                stationDetails.ParkingContentTranslationKey = _context.StationInfoTranslation.Where(x => x.Key == "ParkingContentTranslationKey" && x.StationId == stationId).Select(x => x.Russian).FirstOrDefault();

                break;
        }



                                                                       return stationDetails;
    }
         
                  
      
    public async Task<List<GateServices>> StationGateService(StationInformationRequestDto request)
    {
        var b = (await GetStationInformationAsync(request).ConfigureAwait(false));
        var result = b.GateInfo.SelectMany(x => x.GateServices).DistinctBy(x => x.ServiceName).ToList();// b.GateInfo.SelectMany(x => x.GateServices).GroupBy(x => x.ServiceIcon).Select(x => x.FirstOrDefault()).ToList();
        return result;
    }
    #region AddDeleteUpdate
    public async Task<Station> AddStationAsync(Station stationToAdd)
    {
        Station station = null;


        EntityEntry<Station> entity = await _context.Stations.AddAsync(stationToAdd).ConfigureAwait(false);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            station = entity.Entity;

            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.Stations);
            IEnumerable<Station> stations = await GetStationsNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<Station>>(CacheKeys.Stations, stations).ConfigureAwait(false);
        }


        return station;
    }

    public async Task<bool> DeleteStationAsync(int StationId)
    {
        Station station = await _context.Stations.SingleOrDefaultAsync(p => p.StationId == StationId).ConfigureAwait(false);
        bool success = false;
        if (station != null)
        {
            _ = _context.Stations.Remove(station);
            success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.Stations);
                IEnumerable<Station> stations = await GetStationsNoCache().ConfigureAwait(false);
                await _cacheService.SetAsync<IEnumerable<Station>>(CacheKeys.Stations, stations).ConfigureAwait(false);
            }
        }
        return success;
    }
    public async Task<Station> UpdateStationAsync(Station stationToUpdate)
    {
        await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.Stations).ConfigureAwait(false);
        EntityEntry<Station> entity = _context.Stations.Update(stationToUpdate);
        _context.Entry(stationToUpdate).State = EntityState.Modified;
        IEnumerable<Synonym> synonyms = stationToUpdate.Synonym.ToList();
        IEnumerable<Synonym> allSynonyms = await _context.Synonyms.Where(x => x.StationId == stationToUpdate.StationId).ToListAsync();//.Except(synonyms).ToList();
        var synonymsToRemove = allSynonyms.Where(x => !synonyms.Any(y => y.Id == x.Id));

        foreach (var synonymRem in synonymsToRemove)
        {
            _ = _context.Synonyms.Remove(synonymRem);
        }

        foreach (Synonym item in synonyms)
        {
            item.StationId = stationToUpdate.StationId;
            if (item.Id == 0)
            {
                _ = _context.Synonyms.Add(item);
            }
        }

        if (synonymsToRemove.Count() == 0 && synonyms.Count() == 0)
        {
            foreach (Synonym item in synonyms)
            {
                _ = _context.Synonyms.Update(item);
            }
        }


        var success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        if (success != null)
        {
            IEnumerable<Station> stationsUpdated = await GetStationsNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<Station>>(CacheKeys.Stations, stationsUpdated).ConfigureAwait(false);
        }
        return stationToUpdate;
    }

    public async Task<StationInfo> UpdateNonActiveElavatorsAsync(string nonActiveElavators, int stationId)
    {
        StationInfo stationInfoToUpdate = _context.StationInfo.SingleOrDefault(p => p.StationId == stationId);
        stationInfoToUpdate.NonActiveElavators = nonActiveElavators;

        EntityEntry<StationInfo> entity = _context.StationInfo.Update(stationInfoToUpdate);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.Stations);
            IEnumerable<Station> stations = await GetStationsNoCache().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<Station>>(CacheKeys.Stations, stations).ConfigureAwait(false);
        }

        return stationInfoToUpdate;
    }
    #endregion AddDeleteUpdate
}
