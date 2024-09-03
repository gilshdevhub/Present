using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SpeakersTimer.Dtos;

namespace SpeakersTimer.Controllers;

public class TimerLoginController : BaseApiController
{
    private readonly ITimerLoginService _loginService;
    public TimerLoginController(ITimerLoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<bool>> Login(LoginTimerDto loginDto)
    {
        return Ok(await _loginService.TimerLoginAsync(loginDto.email, loginDto.password));
    }
}
