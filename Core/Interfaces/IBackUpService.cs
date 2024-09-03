using Core.Entities;
using Core.Entities.Push;
using Core.Entities.Translation;
using Core.Entities.Vouchers;

namespace Core.Interfaces;

public interface IBackUpService
{
    Task<IEnumerable<PushNotificationsByWeekDay>> GetPushNotificationsByWeekDay();
    Task<IEnumerable<PushNotificationsByDate>> GetPushNotificationsByDate();
    Task<IEnumerable<StationInformationALL>> GetAllStationInfo();
    Task<IEnumerable<Translation>> GetTranslations();
    Task<IEnumerable<URLTranslation>> GetUrlTranslations();
}
