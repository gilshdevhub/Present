using Core.Entities.Push;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class PushNotificationsService : IPushNotificationsService
{
    private readonly RailDbContext _context;
    private readonly SpecialDbContext _specialContext;
    private readonly ILogger<MessengerService> _logger;
    private readonly IMailService _mailService;

    public PushNotificationsService(RailDbContext context, ILogger<MessengerService> logger, IMailService mailService, SpecialDbContext specialContext)
    {
        _context = context;
        _logger = logger;
        _mailService = mailService;
        _specialContext = specialContext;
    }

    public async Task<PushRegistration> PushRegistrationAsync(PushRegistration item)
    {
        PushRegistration pushRegistration = await _context.PushRegistrations.SingleOrDefaultAsync(p => p.TokenId == item.TokenId).ConfigureAwait(false);

        if (pushRegistration == null)
        {
            var entity = _context.PushRegistrations.Add(item);
            pushRegistration = entity.Entity;
        }
        else
        {
            pushRegistration.State = (int)RegistrationState.Signed;
            pushRegistration.RegistrationDate = DateTime.Now;
            pushRegistration.TokenId = item.TokenId;
            _ = _context.PushRegistrations.Attach(pushRegistration);
            _context.Entry(pushRegistration).State = EntityState.Modified;
        }

        _ = await _context.SaveChangesAsync().ConfigureAwait(false);

        return pushRegistration;
    }

    public async Task<bool> RefreshTokenAsync(TokenRefreshModel model)
    {
        var entities =  await _context.PushRegistrations.Where(e => e.Id == model.Id).ToListAsync();
        var entity = entities.FirstOrDefault( e => e.TokenId == model.OldToken);
        if (entity == null)
        {
            return false;
        }

        entity.TokenId = model.NewToken;
        entity.RefreshDate = DateTime.Now;
        _ = await _context.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

            
                public async Task<bool> PushRegistrationCancelAsync(int id)
    {
        bool result = false;

        PushRegistration pushRegistration = await _context.PushRegistrations
            .Include(p => p.PushRoutings)
            .SingleOrDefaultAsync(p => p.Id == id);

        if (pushRegistration != null)
        {
            pushRegistration.State = (int)RegistrationState.RequestToCancel;
            pushRegistration.CancelDate = DateTime.Now;

            _ = _context.PushRegistrations.Attach(pushRegistration);
            _context.Entry(pushRegistration).State = EntityState.Modified;

                                                       
            result = await _context.SaveChangesAsync() > 0;
        }

        return result;
    }
    public async Task<int> PushNotificationRegistrationAsync(PushRouting pushRouting)
    {
        int pushRouteId = -1;

        PushRegistration pushRegistration = await _context.PushRegistrations.SingleOrDefaultAsync(p => p.Id == pushRouting.PushRegistrationId).ConfigureAwait(false);

        if (pushRegistration != null)
        {
            pushRegistration.PushRoutings.Add(pushRouting);
            _ = await _context.SaveChangesAsync().ConfigureAwait(false);
            pushRouteId = pushRouting.Id;
        }

        return pushRouteId;
    }
    public async Task<bool> PushNotificationCancelAsync(int pushRouteId)
    {
        bool succedded = false;

        PushRouting pushRouting = await _context.PushRouting.SingleOrDefaultAsync(p => p.Id == pushRouteId);

        if (pushRouting != null)
        {
            pushRouting.State = (int)RegistrationState.Canceld;
            _ = _context.PushRouting.Attach(pushRouting);
            _context.Entry(pushRouting).State = EntityState.Modified;

            succedded = await _context.SaveChangesAsync() > 0;
        }

        return succedded;
    }
    public async Task<bool> PushNotificationUpdateAsync(IEnumerable<PushNotificationsByWeekDay> pushNotificationsByWeekDay)
    {
        bool result = false;

        try
        {
            foreach (var item in pushNotificationsByWeekDay)
            {
                _ = _context.PushNotificationsByWeekDay.Update(item);

            }
            result = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }
        catch (Exception ex)
        {
            result = false;
            _logger.LogError(ex, "error occured in PushNotificationUpdateAsync");
                   }

        return result;
    }
    public async Task<PushNotificationResponse> PushNotificationsAsync(PushNotificationQuery pushNotificationQuery)
    {
        PushNotificationResponse pushNotificationResponse = new();


        if (pushNotificationQuery.NotificationType != PushNotificationType.Permanent || pushNotificationQuery.NotificationType == PushNotificationType.All)//תמיד , אם לא נבחר חד פעמי
        {
            IEnumerable<PushNotificationsByDate> result = await PushNotificationByDateAsync(pushNotificationQuery);
            var groups = result.GroupBy(x => x.PushRoutingId);

            pushNotificationResponse.pushNotificationsByDateResponse = new List<PushNotificationsByDateResponse>();


            foreach (var item in groups)
            {
                PushNotificationsByDateResponse pnvwr = new();
                pnvwr.PushNotificationsByDate = new List<PushNotificationsByDate>();
                foreach (var subItem in item)
                {
                    if (subItem.DepartureTime < pnvwr.DepartureTime || pnvwr.DepartureStationId == 0)
                    {
                        pnvwr.DepartureTime = subItem.DepartureTime;
                        pnvwr.DepartureStationId = subItem.DepartureStationId;
                    }
                    if (subItem.ArrivalTime > pnvwr.ArrivalTime)
                    {
                        pnvwr.ArrivalStationId = subItem.ArrivalStationId;
                        pnvwr.ArrivalTime = subItem.ArrivalTime;
                    }
                    pnvwr.PushNotificationsByDate.Add(subItem);
                }
                pushNotificationResponse.pushNotificationsByDateResponse.Add(pnvwr);
            }
        }
        if (pushNotificationQuery.NotificationType != PushNotificationType.OneTime || pushNotificationQuery.NotificationType == PushNotificationType.All)//תמיד , אם לא נבחר קבוע
        {
            IEnumerable<PushNotificationsByWeekDay> result = await PushNotificationByWeekDayAsync(pushNotificationQuery);

            var groups = result.GroupBy(x => x.PushRoutingId);

            pushNotificationResponse.pushNotificationsByWeekDayResponse = new List<PushNotificationsByWeekDayResponse>();


            foreach (var item in groups)
            {
                PushNotificationsByWeekDayResponse pnvwr = new();
                pnvwr.PushNotificationsByWeekDay = new List<PushNotificationsByWeekDay>();
                foreach (var subItem in item)
                {
                    if (subItem.DepartureTime < pnvwr.DepartureTime || pnvwr.DepartureStationId == 0)
                    {
                        pnvwr.DepartureTime = subItem.DepartureTime;
                        pnvwr.DepartureStationId = subItem.DepartureStationId;
                    }
                    if (subItem.ArrivalTime > pnvwr.ArrivalTime)
                    {
                        pnvwr.ArrivalStationId = subItem.ArrivalStationId;
                        pnvwr.ArrivalTime = subItem.ArrivalTime;
                    }
                    pnvwr.PushNotificationsByWeekDay.Add(subItem);
                }
                pushNotificationResponse.pushNotificationsByWeekDayResponse.Add(pnvwr);
            }
        }


        return pushNotificationResponse;
    }
    public async Task<IEnumerable<PushNotificationsByWeekDay>> GetPushNotificationsByWeekDayByRoutintId(int id)
    {
        IEnumerable<PushNotificationsByWeekDay> PNotification = await _context.PushNotificationsByWeekDay.Where(p => p.PushRoutingId == id).ToArrayAsync();
        return PNotification;
    }
    private async Task<IEnumerable<PushNotificationsByDate>> PushNotificationByDateAsync(PushNotificationQuery pushNotificationQuery)
    {
        int pushRoutingState = 0;

        if (pushNotificationQuery.NotificationState.HasValue)
        {
            if (pushNotificationQuery.NotificationState == PushNotificationState.Active)
            {
                pushRoutingState = (int)RegistrationState.Signed;
            }
            else if (pushNotificationQuery.NotificationState == PushNotificationState.Canceled)
            {
                pushRoutingState = (int)RegistrationState.Canceld;
            }
        }

        IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@pushRegistraionId", Value = pushNotificationQuery.PushRegistrationId },
                 new SqlParameter { ParameterName = "@pushRoutingState", Value = pushRoutingState }

            };


        IEnumerable<PushNotificationsByDate> pushNotificationByDate = await _specialContext.PushNotificationsByDate
        .FromSqlRaw("exec [dbo].[notifications_GetPushNotificationsByDate]   @pushRegistraionId, @pushRoutingState", sqlParameters.ToArray())
       .ToArrayAsync().ConfigureAwait(false);
        return pushNotificationByDate;

    }

    private async Task<IEnumerable<PushNotificationsByWeekDay>> PushNotificationByWeekDayAsync(PushNotificationQuery pushNotificationQuery)
    {
        int pushRoutingState = 0;

        if (pushNotificationQuery.NotificationState.HasValue)
        {
            if (pushNotificationQuery.NotificationState == PushNotificationState.Active)
            {
                pushRoutingState = (int)RegistrationState.Signed;
            }
            else if (pushNotificationQuery.NotificationState == PushNotificationState.Canceled)
            {
                pushRoutingState = (int)RegistrationState.Canceld;
            }
        }

        IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@pushRegistraionId", Value = pushNotificationQuery.PushRegistrationId },
                 new SqlParameter { ParameterName = "@pushRoutingState", Value = pushRoutingState }
            };


        IEnumerable<PushNotificationsByWeekDay> pushNotificationByWeekDay = await _specialContext.PushNotificationsByWeekDay
        .FromSqlRaw("exec [dbo].[notifications_GetPushNotificationsByWeekDays]  @pushRegistraionId, @pushRoutingState", sqlParameters.ToArray())
       .ToArrayAsync().ConfigureAwait(false);
        return pushNotificationByWeekDay;

    }


}
