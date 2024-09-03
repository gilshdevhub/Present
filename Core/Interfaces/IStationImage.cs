using Core.Entities.Stations;

namespace Core.Interfaces;

public interface IStationImage
{
    Task<string> GetStationSVGAsync();
    Task<string> BuildSVGAsync();
    Task<bool> UpdateElementActivationAsync(StationImage element, bool closed, DateTime fromdate, bool betweenDatesInactive, DateTime? todate);
    Task<IEnumerable<StationImage>> GetAllSvgElementsAsync();
    Task<object> GetStationSVGPerLanguageAsync();
    Task<IEnumerable<StationImage>> GetStationsImageContentAsync();
}
