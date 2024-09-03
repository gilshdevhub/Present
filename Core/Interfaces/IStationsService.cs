using Core.Entities.Vouchers;

namespace Core.Interfaces;

public interface IStationsService
{
    Task<Station> GetStationAsync(int id);
    Task<Station> GetStationNoCache(int id);
    Task<IEnumerable<Station>> GetStationsAsync();
    Task<IEnumerable<Station>> GetStationsNoCache();
    Task<Station> AddStationAsync(Station stationToAdd);
    Task<Station> UpdateStationAsync(Station stationToUpdate);
    Task<StationInfo> UpdateNonActiveElavatorsAsync(string nonActiveElavators, int stationId);
    Task<bool> DeleteStationAsync(int StationId);
    Task<IEnumerable<StationInfo>> GetStationsInfoAsync();
    Task<StationInfo> GetStationInfoAsync(int id);
    Task<IEnumerable<ParkingCosts>> GetParkingCostsAsync();
    Task<StationInformationRsponseDto> GetStationInformationAsync(StationInformationRequestDto request);
       Task<List<GateServices>> StationGateService(StationInformationRequestDto request);
}
