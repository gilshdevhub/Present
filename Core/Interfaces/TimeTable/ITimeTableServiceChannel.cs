using System.ServiceModel;

namespace Core.Interfaces.TimeTable
{
    public interface ITimeTableServiceChannel : ITimeTable, IClientChannel
    {
    }
}
