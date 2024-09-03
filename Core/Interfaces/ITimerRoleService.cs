using Core.Entities.SpeakerTimer;
using Core.Entities.Translation;
using Core.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Core.Interfaces;

public interface ITimerRoleService
{
    Task<IEnumerable<TimerRole>> GetAllTimerRolesAsync();
    Task<TimerRole> GetTimerRoleByIdAsync(int id);
    Task<bool> DeleteTimerRoleByIdAsync(int id);
    Task<IEnumerable<TimerRole>> AddTimerRoleAsync(TimerRole item);
    Task<IEnumerable<TimerRole>> UpdateTimerRoleAsync(TimerRole TimerRole);
    

}
