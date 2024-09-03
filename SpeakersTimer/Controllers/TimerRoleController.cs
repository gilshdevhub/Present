using Core.Entities.Messenger;
using Core.Entities.SpeakerTimer;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace SpeakersTimer.Controllers;

public class TimerRoleController : BaseApiController
{
    private readonly ITimerRoleService _timerRoleService;
    public TimerRoleController(ITimerRoleService timerRoleService)
    {
        _timerRoleService = timerRoleService;
    }

    [HttpPost("CreateTimerRole")]
    public async Task<ActionResult<IEnumerable<TimerRole>>> CreateTimerRole(TimerRole timerRole)
    {
        return Ok(await _timerRoleService.AddTimerRoleAsync(timerRole));
    }

    [HttpGet("GetAllTimerRoles")]
    public async Task<ActionResult<IEnumerable<TimerRole>>> GetAllTimerRoles()
    {
        return Ok(await _timerRoleService.GetAllTimerRolesAsync());
    }

    [HttpGet("GetTimerRoleById")]
    public async Task<ActionResult<TimerRole>> GetTimerRoleById(int id)
    {
        return Ok(await _timerRoleService.GetTimerRoleByIdAsync(id));
    }

    [HttpGet("DeleteTimerRoleById")]
    public async Task<ActionResult<bool>> DeleteTimerRoleById(int id)
    {
        return Ok(await _timerRoleService.DeleteTimerRoleByIdAsync(id));
    }

    [HttpPut("UpdateTimerRoleById")]
    public async Task<ActionResult<IEnumerable<TimerRole>>> UpdateTimerRole(TimerRole TimerRole)
    {
        return Ok(await _timerRoleService.UpdateTimerRoleAsync(TimerRole));
    }

}
