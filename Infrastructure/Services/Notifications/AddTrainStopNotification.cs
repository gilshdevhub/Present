using Core.Entities.Notifications;
using Core.Entities.Push;
using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using System.Text;

namespace Infrastructure.Services.Notifications;

public class AddTrainStopNotification : BaseNotification
{
    private IEnumerable<Notification> _addTrainStops;

                      public override void ProcessNotification(IEnumerable<AutomationNotification> automationNotifications, PushNotification pushNotification)
    {
        try
        {
                       var addedStationIdsAndOrder = automationNotifications
                .Where(p => RailScheduals.Any(r => r.StationId == p.ChangedStationId))
                .Select(p => new
                {
                    p.Id,
                    AddedStationId = (int)p.ChangedStationId,
                    AddedStationOrder = GetStationOrder(p.TrainDate, p.TrainNumber, (int)p.ChangedStationId),
                    AddedStationArrivalTime = p.ChangedTrainTime.Value,
                    TimeToSend = p.ChangedTrainTime.Value.AddMinutes(-15),
                    p.TrainDate
                }).ToArray();

            int arrivalStationOrder = GetStationOrder(pushNotification.TrainDate, pushNotification.TrainNumber, pushNotification.ArrivalStationId);
            int departureStationOrder = GetStationOrder(pushNotification.TrainDate, pushNotification.TrainNumber, pushNotification.DepartureStationId);

            _addTrainStops = addedStationIdsAndOrder
                .Where(p => (p.AddedStationOrder > departureStationOrder) && (p.AddedStationOrder < arrivalStationOrder) && (p.TrainDate == pushNotification.TrainDate.Date))
                .Select(p => new Notification
                {
                    OriginTime = pushNotification.DepartureTime,
                    TrainNumber = pushNotification.TrainNumber,
                    ArrivalStationName = GetStationName(pushNotification.ArrivalStationId),
                    DepartureStationName = GetStationName(pushNotification.DepartureStationId),
                    AutomationNotificationId = p.Id,
                    UpdatedTime = p.AddedStationArrivalTime,
                    TimeToSend = p.TimeToSend,
                    StationName = GetStationName(p.AddedStationId)
                }).ToArray();

            if (_addTrainStops.Any())
            {
                Notification addTrainStop = _addTrainStops.First();

                base.NotificationEvents.Add(new NotificationEvent
                {
                    Message = CreateMessage(pushNotification, addTrainStop),
                    TimeToSend = addTrainStop.TimeToSend,
                    NotificationTypeId = (int)NotificationTypes.AddTrainStop,
                    PushNotificationId = pushNotification.Id,
                    AutomationNotificationId = addTrainStop.AutomationNotificationId
                });
            }
        }
        catch (Exception ex)
        {
            throw new NotificationException("Notifications - Create notification message - Add train stop", $"push notification id: {pushNotification.Id}", ex);
        }
    }
    public override string CreateMessage(PushNotification pushNotification, Notification notification)
    {
        string messageFormat;
        StringBuilder message = new();

        if (_addTrainStops.Count() == 1)
        {
            messageFormat = GetConfigurationValue(ConfigurationKeys.AddSingleStopMassage);
            _ = message.AppendFormat(messageFormat, pushNotification.DepartureTime.ToIsraelString(), notification.DepartureStationName, notification.ArrivalStationName, notification.TrainNumber, notification.StationName);
        }
        else
        {
            messageFormat = GetConfigurationValue(ConfigurationKeys.AddMultiStopsMassage);
            _ = message.AppendFormat(messageFormat, pushNotification.DepartureTime.ToIsraelString(), notification.DepartureStationName, notification.ArrivalStationName, notification.TrainNumber, "<br>");
            foreach (Notification trainStop in _addTrainStops)
            {
                _ = message.AppendFormat("{0} בשעה {1}<br>", trainStop.StationName, trainStop.UpdatedTime.ToIsraelString());
            }
        }

        return message.ToString();
    }
}
