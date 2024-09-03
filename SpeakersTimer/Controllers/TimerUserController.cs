using Core.Entities.Messenger;
using Core.Entities.SpeakerTimer;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace SpeakersTimer.Controllers;

public class TimerUserController : BaseApiController
{
    private readonly ITimerUserService _timerUserService;
    public TimerUserController(ITimerUserService timerUserService)
    {
        _timerUserService = timerUserService;
    }

    [HttpPost("CreateTimerUser")]
    public async Task<ActionResult<IEnumerable<TimerUser>>> CreateTimerUser(TimerUser timerUser)
    {
        return Ok(await _timerUserService.AddTimerUserAsync(timerUser));
    }

    [HttpGet("GetAllTimerUsers")]
    public async Task<ActionResult<IEnumerable<TimerUser>>> GetAllTimerUsers()
    {
        return Ok(await _timerUserService.GetAllTimerUsersAsync());
    }

    [HttpGet("GetTimerUserById")]
    public async Task<ActionResult<TimerUser>> GetTimerUserById(int id)
    {
        return Ok(await _timerUserService.GetTimerUserByIdAsync(id));
    }

    [HttpGet("DeleteTimerUserById")]
    public async Task<ActionResult<bool>> DeleteTimerUserById(int id)
    {
        return Ok(await _timerUserService.DeleteTimerUserByIdAsync(id));
    }

    [HttpPut("UpdateTimerUserById")]
    public async Task<ActionResult<IEnumerable<TimerUser>>> UpdateTimerUser(TimerUser TimerUser)
    {
        return Ok(await _timerUserService.UpdateTimerUserAsync(TimerUser));
    }

}
