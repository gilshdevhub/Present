using Core.Entities.RailUpdates;
using Core.Enums;

namespace Core.Interfaces;

public interface IRailUpdatesService
{
    Task<RailUpdate> GetRailGeneralUpdatesAsync(Languages languageId);
    Task<IEnumerable<RailUpdateResponseUmbracoDto>> GetRailGeneralUpdatesUmbracoAsync(Languages languageId, SystemTypes systemType);
    Task<List<RailUpdateResponseUmbracoDto>> GetRailGeneralUpdatesUmbracoByStationIdAsync(Languages languageId, int stationId);
    Task<RailUpdate> GetRailSpecialUpdatesAsync(int originStationId, int targerStationId);
    Task<List<RailUpdateResponseUmbracoDto>> AllUmbracoGeneralUpdatesAsync(Languages? languageId);
    Task<List<RailUpdateResponseUmbracoDto>> UmbracoGeneralUpdatesByLanguageAsync(Languages languageId, int stationId);
    Task<IEnumerable<RailUpdateResponseUmbracoDto>> GetRailUpdatesNewAsync(Languages? languageId);
    Task SetRailUpdatesNewAsync();
    Task<bool> SetRailUpdatesWithTimerAsync();
}
