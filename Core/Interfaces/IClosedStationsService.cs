using Core.Entities.Stations;
using Microsoft.AspNetCore.Mvc;

namespace Core.Interfaces;

public interface IClosedStationsService
{
    Task<IEnumerable<ClosedStationsAndLines>> GetClosedStationsAndLinesAsync();
    Task<ActionResult<ClosedStationsAndLines>> GetClosedStationsAndLinesByIdAsync(int StationId);
    Task<ActionResult<bool>> DeleteClosedStationsAndLinesAsync(int StationId);
    Task<bool> UpdateClosedStationsAndLinesAsync(ClosedStationsAndLines stationToUpdate);
    Task<ClosedStationsAndLines> AddClosedStationsAndLinesAsync(ClosedStationsAndLines stationToUpdate);
   }
