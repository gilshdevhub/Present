using Core.Entities.Push;
using Core.Extensions;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class SpecialPushNotificationsService : ISpecialPushNotificationsService
{
    private readonly SpecialDbContext _specialDbContext;
    private readonly RailDbContext _context;
    public SpecialPushNotificationsService(SpecialDbContext specialDbContext, RailDbContext context)
    {
        _specialDbContext = specialDbContext;
        _context = context;
    }
    public async Task<IEnumerable<PushNotificationAndRegistrationIds>> GetPushRegistrationIDs(FilterParametrs parametrs)
    {
        IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@fromDate", Value = parametrs.FromDate},
                new SqlParameter { ParameterName = "@toDate", Value = parametrs.ToDate},
                new SqlParameter { ParameterName = "@trainNumbers", Value = parametrs.TrainNumbers},
                new SqlParameter { ParameterName = "@departureStationId", Value = parametrs.DepartureStationId},
                new SqlParameter { ParameterName = "@arrivalStationId", Value = parametrs.ArrivalStationId }
            };

        IEnumerable<PushNotificationAndRegistrationIds> pushNotificationAndRegistrationIds
            = await _specialDbContext.PushNotificationAndRegistrationIds
           .FromSqlRaw("exec [dbo].[notifications_GetPushRegistrationIDs]  @fromDate, @toDate, @trainNumbers, @departureStationId, @arrivalStationId", sqlParameters.ToArray())
           .ToArrayAsync().ConfigureAwait(false);

        return pushNotificationAndRegistrationIds;
    }

    public async Task<IEnumerable<PushNotificationAndRegistrationIds>> GetPushRegistrationIDsMaslul(FilterParametrsMaslul parametrs)
    {
        IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@fromDate", Value = parametrs.FromDate.ToString("MM-dd-yyyy") },
                new SqlParameter { ParameterName = "@toDate", Value = parametrs.ToDate.ToString("MM-dd-yyyy") },
                new SqlParameter { ParameterName = "@trainNumber", Value = parametrs.TrainNumber},
                new SqlParameter { ParameterName = "@startStationId", Value = parametrs.StartStationId},
                new SqlParameter { ParameterName = "@endStationId", Value = parametrs.EndStationId }
            };

        IEnumerable<PushNotificationAndRegistrationIds> pushNotificationAndRegistrationIds
            = await _specialDbContext.PushNotificationAndRegistrationIds
           .FromSqlRaw("exec [dbo].[notifications_GetPushRegistrationIDsByMaslul]  @fromDate, @toDate, @trainNumber, @startStationId, @endStationId", sqlParameters.ToArray())
           .ToArrayAsync().ConfigureAwait(false);

        return pushNotificationAndRegistrationIds;
    }

            
                     
   
            
          public async Task<IEnumerable<PushNotificationsLog>> GetPushLogAsync(PushLogRequest pushLogRequest)
    {
        IQueryable<PushNotificationsLog> query = _context.PushNotificationsLog.Where(p => p.CreatedDate >= pushLogRequest.FromDate && p.CreatedDate <= pushLogRequest.TillDate);

        var filterMapping = new List<KeyValuePair<bool, Expression<Func<PushNotificationsLog, bool>>>>
        {
            new KeyValuePair<bool, Expression<Func<PushNotificationsLog, bool>>>(pushLogRequest.PushRegistrationId.HasValue, p => p.PushRegistrationId == pushLogRequest.PushRegistrationId)
        };

        query = query.FilterData<PushNotificationsLog>(filterMapping);
        return await query.ToArrayAsync();
    }
}
