using Core.Entities.MotUpdates;

namespace Core.Interfaces;

public interface IMotUpdatesService
{
    Task<MotUpdateResponse> GetMotUpdatesAsync(MotUpdateRequest motUpdateRequest);
    Task<object> GetMotUpdateListsAsync();
    Task<ResponseMotUpdatesByTrainStationDto> GetMotUpdatesByTrainStationsAsync(int stationId, DateTime startTime);
    Task<BLSDto> GetSiriAsync(int stationId);
    Task<IEnumerable<MotConvertion>> GetMotConvertionListContentAsync();
    Task<GTFSBLSDto> GetGTFSAsync(int stationId);
   }
