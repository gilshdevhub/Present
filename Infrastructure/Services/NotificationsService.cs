using Core.Entities.Configuration;
using Core.Entities.Messenger;
using Core.Entities.Notifications;
using Core.Entities.Push;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services.Notifications;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Services;

public class NotificationsService : INotificationsService
{
    private readonly RailDbContext _context;
    private readonly IStationsService _stationsService;
    private readonly ICacheService _cacheService;
    private readonly IConfigurationService _configurationService;
    private readonly ILogger<NotificationsService> _logger;
    private readonly IMailService _mailService;
    private readonly IMessengerService _messengerService;

    public NotificationsService(RailDbContext context, IStationsService stationsService, ICacheService cacheService, IConfigurationService configurationService,
        ILogger<NotificationsService> logger, IMailService mailService, IMessengerService messengerService)
    {
        _context = context;
        _cacheService = cacheService;
        _stationsService = stationsService;
        _configurationService = configurationService;
        _logger = logger;
        _mailService = mailService;
        _messengerService = messengerService;
    }

    public async Task<IEnumerable<NotificationType>> GetNotificationTypesAsync()
    {
        IEnumerable<NotificationType> notificationTypes = await _cacheService.GetAsync<IEnumerable<NotificationType>>(CacheKeys.NotificationTypes).ConfigureAwait(false);

        if (notificationTypes == null || notificationTypes.Count() <= 0)
        {
            notificationTypes = await _context.NotificationTypes.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<NotificationType>>(CacheKeys.NotificationTypes, notificationTypes).ConfigureAwait(false);
        }

        return notificationTypes;
    }
    public async Task<int> ProcessAutomationNotificationAsync(IEnumerable<AutomationNotification> automationNotifications)
    {
        int totalNotificationEvents = 0;

               IEnumerable<int> trainNumbers = automationNotifications.GroupBy(p => p.TrainNumber).Select(p => p.Key).ToArray();

               IEnumerable<PushNotificationInfo> pushNotifications = await GetPushNotificationsAsync(trainNumbers).ConfigureAwait(false);

               IEnumerable<DateTime> pushNotificationDates = pushNotifications.Select(p => p.TrainDate.Date).ToArray();

               trainNumbers = pushNotifications.GroupBy(p => p.TrainNumber).Select(p => p.Key).ToArray();

               automationNotifications = automationNotifications.Where(p => pushNotificationDates.Contains(p.TrainDate) && trainNumbers.Contains(p.TrainNumber)).ToArray();

               IEnumerable<RailSchedual> railScheduals = await _context.RailScheduals.Where(p => pushNotificationDates.Contains(p.TrainDate) && trainNumbers.Contains(p.TrainNumber)).ToArrayAsync().ConfigureAwait(false);

        if (!automationNotifications.Any() || !pushNotifications.Any() || !railScheduals.Any())
        {
            return totalNotificationEvents;
        }

        IEnumerable<Station> stations = await _stationsService.GetStationsAsync().ConfigureAwait(false);
        IEnumerable<ConfigurationParameter> configurations = await _configurationService.GetAllItemsAsync().ConfigureAwait(false);

        foreach (int trainNumber in trainNumbers)
        {
            IEnumerable<PushNotification> trainPushNotifications = pushNotifications.Where(p => p.TrainNumber == trainNumber).ToArray();
            IEnumerable<AutomationNotification> trainNotifications = automationNotifications.Where(p => p.TrainNumber == trainNumber).ToArray();
            IEnumerable<int> notificationTypeIds = trainNotifications.GroupBy(p => p.NotificationTypeId).Select(p => p.Key).ToArray();

            foreach (int notificationTypeId in notificationTypeIds)
            {
                IEnumerable<AutomationNotification> notificationsByNotificationType = trainNotifications.Where(p => p.NotificationTypeId == notificationTypeId).ToArray();

                if (notificationsByNotificationType.Any())
                {
                    BaseNotification baseNotification = NotificationFactory.Create((NotificationTypes)notificationTypeId);
                    baseNotification.Stations = stations;
                    baseNotification.RailScheduals = railScheduals;
                    baseNotification.Configurations = configurations;

                    foreach (PushNotification pushNotification in trainPushNotifications)
                    {
                        try
                        {
                            baseNotification.ProcessNotification(notificationsByNotificationType, pushNotification);
                            foreach (NotificationEvent notificationEvent in baseNotification.NotificationEvents)
                            {
                                totalNotificationEvents += await InsertNotificationEventAsync(notificationEvent).ConfigureAwait(false);
                            }
                        }
                        catch (NotificationException ex)
                        {
                            _logger.LogError(ex.Message, ex);
                                                   }
                    }
                }
            }
        }

        return totalNotificationEvents;
    }
    public async Task<bool> ProcessNotificationEventsAsync(IEnumerable<NotificationEvent> notificationEvents)
    {
        try
        {
            IEnumerable<MessageInfo> messageInfos = notificationEvents.Select(p => new MessageInfo(p.PushRegistrationId.ToString(), p.Message)).ToArray();

            foreach (MessageInfo messageInfo in messageInfos)
            {
                _ = await _messengerService.SendPushAsync(messageInfo).ConfigureAwait(false);
            }

            IEnumerable<int> notificationEventIds = notificationEvents.Select(p => p.Id).ToArray();
            _ = await UpdateNotificationEventsStatusAsync(notificationEventIds, NotificationStatus.Sent).ConfigureAwait(false);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Notifications - Process notification events failed", ex);
                       return false;
        }
    }
    public async Task<IEnumerable<NotificationEventExtraInfo>> GetNotificationEventExtraInfoAsync()
    {
        List<NotificationEventExtraInfo> notificationEventExtraInfo = await GetNotificationEventAsync().ConfigureAwait(false);

        IEnumerable<NotificationEventExtraInfo> updateTrainDelays = notificationEventExtraInfo.Where(p => p.NotificationTypeId == (int)NotificationTypes.UpdateTrainDelay).ToArray();

        foreach (NotificationEventExtraInfo updateTrainDelay in updateTrainDelays)
        {
            IEnumerable<NotificationEventExtraInfo> trainDelays = notificationEventExtraInfo
                .Where(p => p.NotificationTypeId == (int)NotificationTypes.TrainDelay && p.TrainNumber == updateTrainDelay.TrainNumber && p.StationId == updateTrainDelay.StationId)
                .ToArray();
            if (trainDelays.Any())
            {
                IEnumerable<int> notificationEventIds = trainDelays.SelectMany(p => p.Ids.Split(',')).Select(p => int.Parse(p)).ToArray();
                _ = await UpdateNotificationEventsStatusAsync(notificationEventIds, NotificationStatus.CanceledByMachine).ConfigureAwait(false);

                IEnumerable<int> automationNotificationIds = trainDelays.Select(p => p.AutomationNotificationId).ToArray();
                _ = notificationEventExtraInfo.RemoveAll(p => automationNotificationIds.Contains(p.AutomationNotificationId));
            }
        }

        return notificationEventExtraInfo;
    }
    public async Task<bool> UpdateNotificationEventsStatus(IEnumerable<NotificationEvent> notificationEvents)
    {
        IEnumerable<int> notificationEventIds = notificationEvents.Select(p => p.Id).ToArray();
        int updated = await UpdateNotificationEventsStatusAsync(notificationEventIds, notificationEvents.First().Status).ConfigureAwait(false);
        return updated > 0;
    }

    private async Task<int> InsertNotificationEventAsync(NotificationEvent notificationEvent)
    {
        int totalNotificationEvents = 0;

        try
        {
            IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@pushNotificationId", Value = notificationEvent.PushNotificationId },
                new SqlParameter { ParameterName = "@automationNotificationId", Value = notificationEvent.AutomationNotificationId},
                new SqlParameter { ParameterName = "@message", Value = notificationEvent.Message},
                new SqlParameter { ParameterName = "@timeToSend", Value = notificationEvent.TimeToSend},
                new SqlParameter { ParameterName = "@notificationTypeId", Value = notificationEvent.NotificationTypeId},
                new SqlParameter { ParameterName = "@rowEffected", Value = 0,  Direction = System.Data.ParameterDirection.Output }
            };

            const string sql = "exec [dbo].[notifications_InsertNotificationEvent] @pushNotificationId, @automationNotificationId, @message, @timeToSend, @notificationTypeId, @rowEffected output";
            _ = await _context.Database.ExecuteSqlRawAsync(sql, sqlParameters.ToArray()).ConfigureAwait(false);
            totalNotificationEvents = int.Parse(sqlParameters[5].Value.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError("Notificaton - Insert notification event", ex);
                   }

        return totalNotificationEvents;
    }
    private async Task<IEnumerable<PushNotificationInfo>> GetPushNotificationsAsync(IEnumerable<int> trainNumbers)
    {
        string trains = string.Empty;
        trains = trainNumbers.Aggregate(trains, (x, y) => $"{y},{x}");
        trains = trains[0..^1];

        string connectionString = _context.Database.GetConnectionString();

        DataTable dataTable = new();

        using (SqlConnection conn = new(connectionString))
        {
            await conn.OpenAsync().ConfigureAwait(false);

            using SqlCommand command = new();
            command.CommandText = "[dbo].[notifications_GetPushNotifications]";
            command.CommandType = CommandType.StoredProcedure;
            _ = command.Parameters.AddWithValue("@trainNumbers", trains);
            command.Connection = conn;
            dataTable.Load(await command.ExecuteReaderAsync().ConfigureAwait(false));
        }

        List<PushNotificationInfo> pushNotificationInfos = dataTable.AsEnumerable().Select(dr => new PushNotificationInfo
        {
            Id = dr.Field<int>("Id"),
            ArrivalPlatform = dr.Field<int>("ArrivalPlatform"),
            ArrivalStationId = dr.Field<int>("ArrivalStationId"),
            ArrivalTime = dr.Field<DateTime>("ArrivalTime"),
            CreatedDate = dr.Field<DateTime>("CreatedDate"),
            DepartureStationId = dr.Field<int>("DepartureStationId"),
            DepartureTime = dr.Field<DateTime>("DepartureTime"),
            DepartutePlatform = dr.Field<int>("DepartutePlatform"),
            PermanentRegistration = dr.Field<bool>("PermanentRegistration"),
            PushRoutingId = dr.Field<int>("PushRoutingId"),
            TrainNumber = dr.Field<int>("TrainNumber"),
            TrainDate = dr.Field<DateTime>("TrainDate"),
            SelectedDay = dr.Field<int?>("SelectedDay")
        }).ToList();

        return pushNotificationInfos;
    }
    private async Task<List<NotificationEventExtraInfo>> GetNotificationEventAsync()
    {
        string connectionString = _context.Database.GetConnectionString();

        DataTable dataTable = new();

        using (SqlConnection conn = new(connectionString))
        {
            await conn.OpenAsync().ConfigureAwait(false);

            using SqlCommand command = new();
            command.CommandText = "[dbo].[notifications_GetNotificationEventsExtraInfo]";
            command.CommandType = CommandType.StoredProcedure;
            command.Connection = conn;
            dataTable.Load(await command.ExecuteReaderAsync().ConfigureAwait(false));
        }

        List<NotificationEventExtraInfo> notificationEventExtraInfos = dataTable.AsEnumerable().Select(dr => new NotificationEventExtraInfo
        {
            Ids = dr.Field<string>("Ids"),
            AutomationNotificationId = dr.Field<int>("AutomationNotificationId"),
            TrainNumber = dr.Field<int>("TrainNumber"),
            NotificationTypeId = dr.Field<int>("NotificationTypeId"),
            NotificationType = dr.Field<string>("NotificationType"),
            TrainDate = dr.Field<DateTime>("TrainDate"),
            StationId = dr.Field<int>("StationId"),
            StationName = dr.Field<string>("StationName"),
            DepartureTime = dr.Field<DateTime>("DepartureTime"),
            UpdateArrivalTime = dr.Field<DateTime>("UpdateArrivalTime"),
            TimeToSend = dr.Field<DateTime>("TimeToSend"),
            PlatformNumber = dr.Field<int?>("PlatformNumber"),
            Message = dr.Field<string>("Message"),
            StatusName = dr.Field<string>("StatusName"),
            StatusId = dr.Field<int>("StatusId")
        }).ToList();

        return notificationEventExtraInfos;
    }
    private async Task<int> UpdateNotificationEventsStatusAsync(IEnumerable<int> notificationEventIds, NotificationStatus status)
    {
        IEnumerable<NotificationEvent> saveNotificationEvents = await _context.NotificationEvents.Where(p => notificationEventIds.Contains(p.Id))
            .ToArrayAsync().ConfigureAwait(false);

        _context.NotificationEvents.AttachRange(saveNotificationEvents);
        _ = saveNotificationEvents.Select(p => p.Status = status).ToArray();
        return await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
