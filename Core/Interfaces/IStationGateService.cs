using Core.Entities.Vouchers;

namespace Core.Interfaces;

public interface IStationGateService
{
    Task<IEnumerable<StationGateDto>> GetStationGateAsync();
    Task<bool> UpdateStationGateAsync(StationGate stationGateToUpdate, IEnumerable<StationInfoTranslation> stationInfoTranslations);
    Task<bool> InsertStationGateAsync(StationGate stationGateToInsert, IEnumerable<StationInfoTranslation> stationInfoTranslations);
    Task<IEnumerable<Station>> DeleteStationGateAsync(int StationGateId);
    Task<IEnumerable<StationServices>> GetStationServicesAsync();
}
