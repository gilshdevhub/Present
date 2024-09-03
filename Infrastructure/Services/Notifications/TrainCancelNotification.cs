using Core.Entities.Notifications;
using Core.Entities.Push;
using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using System.Text;

namespace Infrastructure.Services.Notifications;

public class TrainCancelNotification : BaseNotification
{
                      public override void ProcessNotification(IEnumerable<AutomationNotification> automationNotifications, PushNotification pushNotification)
    {
        try
        {
            Notification cancelTrain = automationNotifications.Select(p => new Notification
            {
                OriginTime = pushNotification.DepartureTime,
                TrainNumber = p.TrainNumber,
                ArrivalStationName = GetStationName(pushNotification.ArrivalStationId),
                DepartureStationName = GetStationName(pushNotification.DepartureStationId),
                AutomationNotificationId = p.Id,
                TimeToSend = pushNotification.DepartureTime.Subtract(p.CreateDate).Hours > 24 ? pushNotification.DepartureTime.AddHours(-24) : p.CreateDate
            }).FirstOrDefault();

            if (cancelTrain != null)
            {
                base.NotificationEvents.Add(new NotificationEvent
                {
                    Message = CreateMessage(pushNotification, cancelTrain),
                    TimeToSend = cancelTrain.TimeToSend,
                    PushNotificationId = pushNotification.Id,
                    NotificationTypeId = (int)NotificationTypes.TrainCancel,
                    AutomationNotificationId = cancelTrain.AutomationNotificationId
                });
            }
        }
        catch (Exception ex)
        {
            throw new NotificationException("Notifications - Create notification message - Train Cancel", $"push notification id: {pushNotification.Id}", ex);
        }
    }
    public override string CreateMessage(PushNotification pushNotification, Notification notification)
    {
        string messageFormat;
        StringBuilder message = new();

        messageFormat = GetConfigurationValue(ConfigurationKeys.TrainCancelMessage);
        _ = message.AppendFormat(messageFormat, notification.OriginTime.ToIsraelString(), notification.DepartureStationName, notification.ArrivalStationName, notification.TrainNumber, "url");

        return message.ToString();
    }
}
