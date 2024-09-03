using Core.Entities.Vouchers;

namespace Core.Interfaces;

public interface IStationGateActivityHoursService
{
    Task<IEnumerable<StationGateActivityHoursDto>> GetStationGateActivityHoursAsync();
    Task<Dictionary<int, List<StationGateActivityHoursDto>>> GetHoursByStationAndTemplateTypeAsync(int stationId, int templateTypeId, IEnumerable<StationGateDto> stationGates);
    Task<List<string>> PreAddStationGateActivityHour(List<int> StationGatesIds);
    Task<bool> AddStationGateActivityHourAsync(StationGateActivityHours StationGateActivityHours, IEnumerable<StationGateActivityHoursLines> StationGateActivityHoursLines);
    Task<bool> UpdateStationGateActivityHourAsync(StationGateActivityHours StationGateActivityHours, IEnumerable<StationGateActivityHoursLines> StationGateActivityHoursLines);
    Task<bool> DeleteGateActivityHourAsync(int Id);
    Task<IEnumerable<StationGateActivityHoursDto>> GetStationGateActivityHoursByGateIdTemplateIdAsync(int templateTypeId, int stationGateId);
    Task<int> StationHoursIdByGateId(int gateId, int templateTypeid);
}
