using Core.Entities.AppMessages;

namespace Core.Interfaces;

public interface ITrainWarningsService
{
    Task<IEnumerable<TrainWarning>> GetTrainWarningsAsync();
    Task<IEnumerable<WarningType>> GetWarningTypesAsync();
    Task<bool> AddTrainWarningAsync(TrainWarning trainWarning);
    Task<bool> UpdateTrainWarningAsync(TrainWarning trainWarningToUpdate);
    Task<bool> DeleteTrainWarningAsync(int Id);

}
