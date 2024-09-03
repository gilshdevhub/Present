using Core.Entities.SpeakerTimer;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TimerLoginService : ITimerLoginService
{
    private readonly RailDbContext _context;

    public TimerLoginService(RailDbContext context)
    {
        _context = context;
    }

    public async Task<bool> TimerLoginAsync(string email, string password)
    {
        var item = await _context.TimerUser.Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();

        if (item != null)
        {
            return true;
        }
        else { return false; }
    }
}
