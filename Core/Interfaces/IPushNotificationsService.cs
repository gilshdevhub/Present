using Core.Entities.Push;

namespace Core.Interfaces;

public interface IPushNotificationsService
{
    Task<PushRegistration> PushRegistrationAsync(PushRegistration item);
    Task<bool> PushRegistrationCancelAsync(int id);
    Task<bool> RefreshTokenAsync(TokenRefreshModel model);
    Task<int> PushNotificationRegistrationAsync(PushRouting pushRouting);
    Task<bool> PushNotificationCancelAsync(int pushRouteId);
    Task<bool> PushNotificationUpdateAsync(IEnumerable<PushNotificationsByWeekDay> pushNotificationsByWeekDay);
    Task<PushNotificationResponse> PushNotificationsAsync(PushNotificationQuery pushNotificationQuery);
    Task<IEnumerable<PushNotificationsByWeekDay>> GetPushNotificationsByWeekDayByRoutintId(int id);
}