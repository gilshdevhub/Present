using Core.Entities.FreeSeats;

namespace Core.Interfaces;

public interface IFreeSeatsService
{
    Task<FreeSeats> GetFreeSeatsAsync(FreeSeatsRequest request);
}
