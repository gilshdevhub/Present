using API.Dtos.Push;
using AutoMapper;
using Core.Entities.Push;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class PushController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IPushNotificationsService _pushNotificationsService;

    public PushController(IMapper mapper, IPushNotificationsService pushNotificationsService)
    {
        _mapper = mapper;
        _pushNotificationsService = pushNotificationsService;
    }

    [HttpPost("registration/create")]
    public async Task<ActionResult<PushRegistrationResponseDto>> PushRegistration(PushRegistrationRequestDto request)
    {
        PushRegistration pushRegistration = _mapper.Map<PushRegistration>(request);
        pushRegistration = await _pushNotificationsService.PushRegistrationAsync(pushRegistration).ConfigureAwait(false);
        PushRegistrationResponseDto response = _mapper.Map<PushRegistrationResponseDto>(pushRegistration);
        return Ok(response);
    }

    [HttpPost("registration/refresh")]
    public async Task<ActionResult<bool>> RefreshToken(TokenRefreshModel model)
    {
        bool res = await _pushNotificationsService.RefreshTokenAsync(model).ConfigureAwait(false);
       
        return Ok(res);
    }

    [HttpPut("registration/cancel/{id}")]
    public async Task<ActionResult<bool>> PushRegistrationCancel([FromRoute] int id)
    {
        bool result = await _pushNotificationsService.PushRegistrationCancelAsync(id).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("notifications/cancel/{id}")]
    public async Task<ActionResult<bool>> PushNotificationsCancel([FromRoute] int id)
    {
        bool result = await _pushNotificationsService.PushNotificationCancelAsync(id).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("notifications/createbydate")]
    public async Task<ActionResult<int>> PushNotificationsByDate(PushNotificationsByDateDto request)
    {
        PushRouting pushRouting = _mapper.Map<PushRouting>(request);
        pushRouting.PermanentRegistration = 0;
        int pushRouteId = await _pushNotificationsService.PushNotificationRegistrationAsync(pushRouting).ConfigureAwait(false);
        return Ok(new PushNotificationResponseDto { PushRoutingId = pushRouteId });
    }
    
    [HttpPost("notifications/createbyweekday")]
    public async Task<ActionResult<int>> PushNotificationsByWeekDay(PushNotificationsByWeekDayDto request)
    {
        PushRouting pushRouting = _mapper.Map<PushRouting>(request);
        pushRouting.PermanentRegistration = 1;
        int pushRouteId = await _pushNotificationsService.PushNotificationRegistrationAsync(pushRouting).ConfigureAwait(false);
        return Ok(new PushNotificationResponseDto { PushRoutingId = pushRouteId });
    }

    [HttpPut("notifications/update")]
    public async Task<ActionResult<bool>> PushNotificationsUpdate(PushNotificationUpdateRequestDto request)
    {
        IEnumerable<PushNotificationsByWeekDay> Pnbyweek = await _pushNotificationsService.GetPushNotificationsByWeekDayByRoutintId(request.PushRoutingId);
        foreach (var item in Pnbyweek)
        {
            item.day1 = request.WeekDays.day1;
            item.day2 = request.WeekDays.day2;
            item.day3 = request.WeekDays.day3;
            item.day4 = request.WeekDays.day4;
            item.day5 = request.WeekDays.day5;
            item.day6 = request.WeekDays.day6;
            item.day7 = request.WeekDays.day7;
        }
        
        bool result = await _pushNotificationsService.PushNotificationUpdateAsync(Pnbyweek).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("notifications/search")]
    public async Task<ActionResult<IEnumerable<PushNotificationResponse>>> PushNotificationsSearch([FromQuery] PushNotificationRequestQueryDto pushNotificationRequestQueryDto)
    {
        PushNotificationQuery pushNotificationQuery = _mapper.Map<PushNotificationQuery>(pushNotificationRequestQueryDto);
        PushNotificationResponse pushNotificationResponse =  await _pushNotificationsService.PushNotificationsAsync(pushNotificationQuery).ConfigureAwait(false);
         return Ok(pushNotificationResponse);
    }
}
