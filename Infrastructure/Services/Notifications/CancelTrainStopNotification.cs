using Core.Entities.Notifications;
using Core.Entities.Push;
using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using System.Text;

namespace Infrastructure.Services.Notifications;

public class CancelTrainStopNotification : BaseNotification
{
    IEnumerable<Notification> _cancelTrainStops;

                      public override void ProcessNotification(IEnumerable<AutomationNotification> automationNotifications, PushNotification pushNotification)
    {
        try
        {
            #region תחנת העליה של הלקוח בוטלה
            _cancelTrainStops = automationNotifications
                .Where(p => p.ChangedStationId == pushNotification.DepartureStationId)
                .Select(p => new Notification
                {
                    DepartureStationName = GetStationName(pushNotification.DepartureStationId),
                    ArrivalStationName = GetStationName(pushNotification.ArrivalStationId),
                    StationName = GetStationName((int)p.ChangedStationId),
                    MassageType = ConfigurationKeys.CancelDepartureStationMassage,
                    OriginTime = pushNotification.DepartureTime,
                    TrainNumber = pushNotification.TrainNumber,
                    AutomationNotificationId = p.Id,
                    TimeToSend = GetTimeToSend(p, pushNotification)
                }).ToArray();

            if (_cancelTrainStops.Any())
            {
                base.NotificationEvents.Add(CreateNotificationEvent(pushNotification));
            }
            #endregion

            #region תחנת הירידה של הלקוח בוטלה
            _cancelTrainStops = automationNotifications.Where(p => p.ChangedStationId == pushNotification.ArrivalStationId)
                .Select(p => new Notification
                {
                    AutomationNotificationId = p.Id,
                    TimeToSend = GetTimeToSend(p, pushNotification),
                    StationName = GetStationName((int)p.ChangedStationId),
                    DepartureStationName = GetStationName(pushNotification.DepartureStationId),
                    ArrivalStationName = GetStationName(pushNotification.ArrivalStationId),
                    OriginTime = pushNotification.DepartureTime,
                    TrainNumber = pushNotification.TrainNumber,
                    MassageType = ConfigurationKeys.CancelArrivalStationMassage
                }).ToArray();

            if (_cancelTrainStops.Any())
            {
                base.NotificationEvents.Add(CreateNotificationEvent(pushNotification));
            }
            #endregion

            #region ביטול עצירה\ות על תחנה\ות שאינה תחנת עליה או ירידה
            var cancelledStationIdsAndOrder = automationNotifications.Select(p => new
            {
                p.Id,
                CancelledStationId = (int)p.ChangedStationId,
                CancelledStationOrder = GetStationOrder(p.TrainDate, p.TrainNumber, (int)p.ChangedStationId),
                TimeToSend = GetTrainTime(p.TrainDate, p.TrainNumber, (int)p.ChangedStationId).AddMinutes(-15)
            }).ToArray();

            int arrivalStationOrder = GetStationOrder(pushNotification.TrainDate, pushNotification.TrainNumber, pushNotification.ArrivalStationId);
            int departureStationOrder = GetStationOrder(pushNotification.TrainDate, pushNotification.TrainNumber, pushNotification.DepartureStationId);

            _cancelTrainStops = cancelledStationIdsAndOrder
                .Where(p => p.CancelledStationOrder > departureStationOrder && p.CancelledStationOrder < arrivalStationOrder)
                .Select(p => new Notification
                {
                    AutomationNotificationId = p.Id,
                    StationName = GetStationName(p.CancelledStationId),
                    OriginTime = pushNotification.DepartureTime,
                    ArrivalStationName = GetStationName(pushNotification.ArrivalStationId),
                    TrainNumber = pushNotification.TrainNumber,
                    TimeToSend = p.TimeToSend
                }).ToArray();

            if (_cancelTrainStops.Any())
            {
                base.NotificationEvents.Add(CreateNotificationEvent(pushNotification));
            }
            #endregion
        }
        catch (Exception ex)
        {
            throw new NotificationException("Notifications - Create notification message - Cancel train stop", $"push notification id: {pushNotification.Id}", ex);
        }
    }
    public override string CreateMessage(PushNotification pushNotification, Notification notification)
    {
        string messageFormat;
        StringBuilder message = new();

        if (notification.MassageType == ConfigurationKeys.CancelDepartureStationMassage)
        {
            #region תחנת עליה של הלקוח בוטלה
            messageFormat = GetConfigurationValue(ConfigurationKeys.CancelDepartureStationMassage);
            _ = message.AppendFormat(messageFormat, notification.OriginTime.ToIsraelString(), notification.DepartureStationName, notification.ArrivalStationName, notification.TrainNumber, "url");
            #endregion
        }
        else if (notification.MassageType == ConfigurationKeys.CancelArrivalStationMassage)
        {
            #region תחנת ירידה של הלקוח בוטלה
            messageFormat = GetConfigurationValue(ConfigurationKeys.CancelArrivalStationMassage);
            _ = message.AppendFormat(messageFormat, notification.OriginTime.ToIsraelString(), notification.DepartureStationName, notification.TrainNumber, notification.StationName, "url");
            #endregion
        }
        else if (_cancelTrainStops.Count() == 1)
        {
            #region ביטול עצירה אחת
            messageFormat = GetConfigurationValue(ConfigurationKeys.CancelSingleStopMassage);
            _ = message.AppendFormat(messageFormat, notification.OriginTime.ToIsraelString(), notification.ArrivalStationName, notification.TrainNumber, notification.StationName);
            #endregion
        }
        else
        {
            #region ביטול מספר עצירות
            messageFormat = GetConfigurationValue(ConfigurationKeys.CancelMultiStopsMassage);
            _ = message.AppendFormat(messageFormat, notification.OriginTime.ToIsraelString(), notification.ArrivalStationName, notification.TrainNumber);
            foreach (Notification trainStation in _cancelTrainStops)
            {
                _ = message.Append(trainStation.StationName).Append("<br>");
            }
            #endregion
        }

        return message.ToString();
    }

    private static DateTime GetTimeToSend(AutomationNotification automationNotification, PushNotification pushNotification)
    {
        DateTime timeToSend = DateTime.Now;

        if (automationNotification.CreateDate.Date == pushNotification.DepartureTime.Date)
        {
                       DateTime startWarningRecieveTime = automationNotification.CreateDate.Date.AddHours(0);
            DateTime endWarningRecieveTime = automationNotification.CreateDate.Date.AddHours(7);
            DateTime startTravelTime = pushNotification.DepartureTime.Date.AddHours(20);
            DateTime endTravelTime = pushNotification.DepartureTime.Date.AddHours(24);

            if (automationNotification.CreateDate > startWarningRecieveTime && automationNotification.CreateDate < endWarningRecieveTime &&
                (pushNotification.DepartureTime > startTravelTime && pushNotification.DepartureTime < endTravelTime))
            {
                timeToSend = endWarningRecieveTime;
            }
        }
        else if (automationNotification.CreateDate.Date < pushNotification.DepartureTime.Date)
        {
                       DateTime startTravelTime1 = pushNotification.DepartureTime.Date;
            DateTime endTravelTime1 = pushNotification.DepartureTime.Date.AddHours(7);
            DateTime startTravelTime2 = pushNotification.DepartureTime.Date.AddHours(7);
            DateTime endTravelTime2 = pushNotification.DepartureTime.Date.AddHours(24);

            if (pushNotification.DepartureTime > startTravelTime1 && pushNotification.DepartureTime < endTravelTime1)
            {
                timeToSend = automationNotification.CreateDate.Date.AddHours(7);
            }
            else if (pushNotification.DepartureTime > startTravelTime2 && pushNotification.DepartureTime < endTravelTime2)
            {
                timeToSend = pushNotification.DepartureTime.AddHours(-24);
            }
        }

        return timeToSend;
    }
    private NotificationEvent CreateNotificationEvent(PushNotification pushNotification)
    {
        Notification cancelTrainStop = _cancelTrainStops.First();

        return new NotificationEvent
        {
            Message = CreateMessage(pushNotification, cancelTrainStop),
            TimeToSend = cancelTrainStop.TimeToSend,
            PushNotificationId = pushNotification.Id,
            NotificationTypeId = (int)NotificationTypes.CancelTrainStop,
            AutomationNotificationId = cancelTrainStop.AutomationNotificationId
        };
    }
}
