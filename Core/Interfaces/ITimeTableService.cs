using Core.Entities.TimeTable;

namespace Core.Interfaces;

public interface ITimeTableService
{
    Task<TrainTimeTableRespnse> GetTrainTimeTableAsync(TrainTimeTableRequest request);
    Task<TrainTimeTableRespnse> GetTrainTimeTableBeforeDateAsync(TrainTimeTableRequest request);
    Task<TrainTimeTableRespnse> GetTrainTimeTableAfterDateAsync(TrainTimeTableRequest request);
    Task<TrainTimeTableRespnse> GetTrainTimeTableByTrainNumberAsync(TrainTimeTableByTrainNumberRequest request);
}
