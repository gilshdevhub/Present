using Core.Entities.MyTravel;
using Core.Entities.Vouchers;
using static Core.Entities.Vouchers.MyTrip;

namespace Core.Interfaces;

public interface IMyTripService
{
    Task<IEnumerable<VisaTrainMainData>> GetClosestSations(ClosedTrainsRequestDto request);
}
