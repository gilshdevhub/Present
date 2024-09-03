using Core.Entities.SpeakerTimer;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TimerRoleService : ITimerRoleService
{
    private readonly RailDbContext _context;

    public TimerRoleService(RailDbContext context)
    {
        _context = context;
    }

   
    public async Task<IEnumerable<TimerRole>> GetAllTimerRolesAsync()
    {
        return await _context.TimerRole.ToListAsync();
    }
    public async Task<TimerRole> GetTimerRoleByIdAsync(int id)
    {
        return await _context.TimerRole.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    public async Task<bool> DeleteTimerRoleByIdAsync(int id)
    {
        TimerRole item = await _context.TimerRole.Where(x => x.Id == id).FirstOrDefaultAsync();
        bool success = false;
        if (item != null)
        {
            _ = _context.TimerRole.Remove(item);
            success = await _context.SaveChangesAsync() > 0;
          
        }

        return success;
    }
    public async Task<IEnumerable<TimerRole>> AddTimerRoleAsync(TimerRole item)
    {
        var entity = _context.TimerRole.Add(item);
        TimerRole timerRole = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0; 

        if (success)
        {
            IEnumerable<TimerRole> timerRoles = await _context.TimerRole
                .ToArrayAsync().ConfigureAwait(false);
            return timerRoles;
        }
        else
        {
            return null;
        }
    }

    public async Task<IEnumerable<TimerRole>> UpdateTimerRoleAsync(TimerRole item)
    {
        IEnumerable<TimerRole> TimerRole = null;
        _ = _context.TimerRole.Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            TimerRole = await _context.TimerRole.ToArrayAsync().ConfigureAwait(false);
        }
        return TimerRole;
    }
}
