using Core.Entities.SpeakerTimer;
using Core.Entities.Translation;
using Core.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.Interfaces;

public interface ITimerUserService
{
    Task<IEnumerable<TimerUser>> GetAllTimerUsersAsync();
    Task<TimerUser> GetTimerUserByIdAsync(int id);
    Task<bool> DeleteTimerUserByIdAsync(int id);
    Task<IEnumerable<TimerUser>> AddTimerUserAsync(TimerUser item);
    Task<IEnumerable<TimerUser>> UpdateTimerUserAsync(TimerUser TimerUser);
    

}
