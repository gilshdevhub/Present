using Core.Entities.Push;

namespace Core.Interfaces;

public interface ISpecialPushNotificationsService
{
    Task<IEnumerable<PushNotificationAndRegistrationIds>> GetPushRegistrationIDs(FilterParametrs parametrs);
    Task<IEnumerable<PushNotificationAndRegistrationIds>> GetPushRegistrationIDsMaslul(FilterParametrsMaslul parametrs);
    Task<IEnumerable<PushNotificationsLog>> GetPushLogAsync(PushLogRequest pushLogRequest);
}
