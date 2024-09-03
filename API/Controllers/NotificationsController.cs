using Core.Entities.Notifications;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class NotificationsController : BaseApiController
{
    private readonly INotificationsService _notificationsService;

    public NotificationsController(INotificationsService notificationsService)
    {
        _notificationsService = notificationsService;
    }

    [HttpPost("automations")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<int>> ProcessAutomationNotification([FromBody] IEnumerable<AutomationNotification> automationNotifications)
    {
        int notificationEvents = await _notificationsService.ProcessAutomationNotificationAsync(automationNotifications).ConfigureAwait(false);
        return Ok(notificationEvents);
    }

    [HttpPut("events")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<bool>> ProcessNotificationEvents([FromBody] IEnumerable<NotificationEvent> notificationEvents)
    {
        bool isSuccess = await _notificationsService.ProcessNotificationEventsAsync(notificationEvents).ConfigureAwait(false);
        return Ok(isSuccess);
    }

    [HttpPut("updateStatus")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ServiceFilter(typeof(WriteToLogFilterAttribute))]
    public async Task<ActionResult<bool>> UpdateNotificationEventsStatus([FromBody] IEnumerable<NotificationEvent> notificationEvents)
    {
        bool isSuccess = await _notificationsService.UpdateNotificationEventsStatus(notificationEvents).ConfigureAwait(false);
        return Ok(isSuccess);
    }

    [HttpGet("events")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ServiceFilter(typeof(WriteToLogFilterAttribute))]
    public async Task<ActionResult<IEnumerable<NotificationEventExtraInfo>>> GetNotificationEventExtraInfo()
    {
        IEnumerable<NotificationEventExtraInfo> notificationEventExtraInfos = await _notificationsService.GetNotificationEventExtraInfoAsync().ConfigureAwait(false);
        return Ok(notificationEventExtraInfos);
    }
}
