using Core.Entities.SpeakerTimer;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ParticipantService : IParticipantService
{
    private readonly RailDbContext _context;

    public ParticipantService(RailDbContext context)
    {
        _context = context;
    }
   
    public async Task<IEnumerable<Participant>> GetAllParticipantsAsync()
    {
        return await _context.Participant.ToListAsync();
    }
    public async Task<Participant> GetParticipantByIdAsync(int id)
    {
        return await _context.Participant.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    public async Task<bool> DeleteParticipantByIdAsync(int id)
    {
        Participant item = await _context.Participant.Where(x => x.Id == id).FirstOrDefaultAsync();
        bool success = false;
        if (item != null)
        {
            _ = _context.Participant.Remove(item);
            success = await _context.SaveChangesAsync() > 0;
          
        }

        return success;
    }
    public async Task<IEnumerable<Participant>> AddParticipantAsync(Participant item)
    {
        var entity = _context.Participant.Add(item);
        Participant participant = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0; 

        if (success)
        {
            IEnumerable<Participant> participants = await _context.Participant
                .ToArrayAsync().ConfigureAwait(false);
            return participants;
        }
        else
        {
            return null;
        }
    }

    public async Task<IEnumerable<Participant>> UpdateParticipantAsync(Participant item)
    {
        IEnumerable<Participant> Participant = null;
        _ = _context.Participant.Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            Participant = await _context.Participant.ToArrayAsync().ConfigureAwait(false);
        }
        return Participant;
    }
}
