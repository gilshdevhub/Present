using Core.Entities;
using Core.Entities.PisData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;

public interface IPisData
{
    Task<Siri> GetPisDataAsync();
    Task<IEnumerable<VehicleActivity>> GetPisDataByOriginRefAsync(int OriginRef);
    Task<IEnumerable<VehicleActivity>> GetPisDataByDestinationRefAsync(int DestinationRef);
    Task<IEnumerable<VehicleActivity>> GetPisDataByStopPointRefAsync(int StopPointRef);
    Task<IEnumerable<VehicleActivity>> GetPisDataByVehicleRefAsync(int VehicleRef);
    Task<IEnumerable<VehicleActivity>> GetPisDataByStationIdAsync(int StationId);
    Task<bool> SetPisDataCacheAsync(Siri PisData);
}
