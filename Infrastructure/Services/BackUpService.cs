using Core.Entities;
using Core.Entities.Push;
using Core.Entities.Translation;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class BackUpService : IBackUpService
{
    private readonly RailDbContext _context;
    private readonly IStationsService _stationsService;

    public BackUpService(RailDbContext context, IStationsService stationsService)
    {
        _context = context;
        _stationsService = stationsService;
    }

    public async Task<IEnumerable<PushNotificationsByWeekDay>> GetPushNotificationsByWeekDay()
    {
        return await _context.PushNotificationsByWeekDay.ToArrayAsync().ConfigureAwait(false);

    }

    public async Task<IEnumerable<PushNotificationsByDate>> GetPushNotificationsByDate()
    {
        return await _context.PushNotificationsByDate.ToArrayAsync().ConfigureAwait(false);

    }

    public async Task<IEnumerable<StationInformationALL>> GetAllStationInfo()
    {
        List<StationInformationALL> allLangAndIds = new List<StationInformationALL>();
        IEnumerable<int> ids = await _context.Stations.Select(x => x.StationId).ToArrayAsync();

        foreach (var id in ids)
        {
            StationInformationALL all = new StationInformationALL();
            StationInformationRequestDto request = new StationInformationRequestDto();
            request.StationId = id;
            request.SystemType = 2;

            request.LanguageId = Languages.English;
            all.english = await _stationsService.GetStationInformationAsync(request).ConfigureAwait(false);

            request.LanguageId = Languages.Hebrew;
            all.hebrew = await _stationsService.GetStationInformationAsync(request).ConfigureAwait(false);

            request.LanguageId = Languages.Arabic;
            all.arabic = await _stationsService.GetStationInformationAsync(request).ConfigureAwait(false);

            request.LanguageId = Languages.Russian;
            all.russian = await _stationsService.GetStationInformationAsync(request).ConfigureAwait(false);

            allLangAndIds.Add(all);
        }
        return allLangAndIds;
    }

    public async Task<IEnumerable<Translation>> GetTranslations()
    {
        return await _context.Translations.Where(x=>x.SystemType.Id==2).ToArrayAsync();
    }

    public async Task<IEnumerable<URLTranslation>> GetUrlTranslations()
    {
        return await _context.URLTranslations.Where(x => x.SystemType.Id == 2).ToArrayAsync();
    }

}
