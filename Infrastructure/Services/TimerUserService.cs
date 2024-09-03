using Core.Entities.SpeakerTimer;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TimerUserService : ITimerUserService
{
    private readonly RailDbContext _context;

    public TimerUserService(RailDbContext context)
    {
        _context = context;
    }

   
    public async Task<IEnumerable<TimerUser>> GetAllTimerUsersAsync()
    {
        return await _context.TimerUser.ToListAsync();
    }
    public async Task<TimerUser> GetTimerUserByIdAsync(int id)
    {
        return await _context.TimerUser.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    public async Task<bool> DeleteTimerUserByIdAsync(int id)
    {
        TimerUser item = await _context.TimerUser.Where(x => x.Id == id).FirstOrDefaultAsync();
        bool success = false;
        if (item != null)
        {
            _ = _context.TimerUser.Remove(item);
            success = await _context.SaveChangesAsync() > 0;
          
        }

        return success;
    }
    public async Task<IEnumerable<TimerUser>> AddTimerUserAsync(TimerUser item)
    {
        var entity = _context.TimerUser.Add(item);
        TimerUser timerUser = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0; 

        if (success)
        {
            IEnumerable<TimerUser> timerUsers = await _context.TimerUser
                .ToArrayAsync().ConfigureAwait(false);
            return timerUsers;
        }
        else
        {
            return null;
        }
    }

    public async Task<IEnumerable<TimerUser>> UpdateTimerUserAsync(TimerUser item)
    {
        IEnumerable<TimerUser> TimerUser = null;
        _ = _context.TimerUser.Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            TimerUser = await _context.TimerUser.ToArrayAsync().ConfigureAwait(false);
        }
        return TimerUser;
    }
}
