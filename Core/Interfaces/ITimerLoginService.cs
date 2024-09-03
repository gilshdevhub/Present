using Core.Entities.SpeakerTimer;
using Core.Entities.Translation;
using Core.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.Interfaces;

public interface ITimerLoginService
{
    Task<bool> TimerLoginAsync(string email, string password);
}
