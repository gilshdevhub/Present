using Core.Entities.Configuration;
using Core.Entities.Notifications;
using Core.Entities.Push;
using Core.Entities.Vouchers;

namespace Infrastructure.Services.Notifications;

public abstract class BaseNotification : IBaseNotification
{
    protected BaseNotification()
    {
        this.NotificationEvents = new();
    }

    public IEnumerable<RailSchedual> RailScheduals { get; set; }
    public IEnumerable<Station> Stations { get; set; }
    public IEnumerable<ConfigurationParameter> Configurations { get; set; }
    public List<NotificationEvent> NotificationEvents { get; set; }

    public virtual void ProcessNotification(IEnumerable<AutomationNotification> automationNotifications, PushNotification pushNotification)
    {
        throw new NotImplementedException();
    }

    public virtual string CreateMessage(PushNotification pushNotification, Notification notification)
    {
        throw new NotImplementedException();
    }

    protected int GetStationOrder(DateTime trainDate, int trainNumber, int stationId)
    {
        return RailScheduals.Single(p => p.TrainDate.Date == trainDate.Date && p.TrainNumber == trainNumber && p.StationId == stationId).StationOrder;
    }
    protected int GetTrainPlatform(DateTime trainDate, int trainNumber, int stationId)
    {
        return RailScheduals.Single(p => p.TrainDate.Date == trainDate.Date && p.TrainNumber == trainNumber && p.StationId == stationId).Platform;
    }
    protected string GetStationName(int stationId)
    {
        return Stations.Single(p => p.StationId == stationId).HebrewName;
    }
    protected string GetConfigurationValue(string key)
    {
        return Configurations.Single(p => p.Key == key).ValueMob;
    }
    protected DateTime GetTrainTime(DateTime trainDate, int trainNumber, int stationId)
    {
        RailSchedual railSchedual = RailScheduals.Single(p => p.TrainDate == trainDate && p.TrainNumber == trainNumber && p.StationId == stationId);
        DateTime trainTime = railSchedual.ArrivalTime.HasValue ? (DateTime)railSchedual.ArrivalTime : (DateTime)railSchedual.DepartureTime;
        return trainTime;
    }
}
