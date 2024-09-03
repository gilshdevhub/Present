using Core.Entities.AppMessages;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class TrainWarningsService : ITrainWarningsService
{
    private readonly RailDbContext _context;

    public TrainWarningsService(RailDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TrainWarning>> GetTrainWarningsAsync()
    {
        IEnumerable<TrainWarning> trainWarnings = await _context.TrainWarnings
            .Include(p => p.WarningType).Include(p => p.SystemType).ToArrayAsync().ConfigureAwait(false);

        return trainWarnings;
    }

    public async Task<bool> AddTrainWarningAsync(TrainWarning trainWarning)
    {
        _ = await _context.TrainWarnings.AddAsync(trainWarning);

        bool success = await _context.SaveChangesAsync() > 0;
        return success;
    }

    public async Task<bool> UpdateTrainWarningAsync(TrainWarning trainWarningToUpdate)
    {
        _ = _context.TrainWarnings.Attach(trainWarningToUpdate);
        _context.Entry(trainWarningToUpdate).State = EntityState.Modified;

        return await CompleteAsync() > 0;
    }

    private async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }
    public async Task<IEnumerable<WarningType>> GetWarningTypesAsync()
    {
        return await _context.WarningType.ToArrayAsync();
    }

    public async Task<bool> DeleteTrainWarningAsync(int Id)
    {
        TrainWarning trainWarning = await _context.TrainWarnings.SingleOrDefaultAsync(p => p.Id == Id);
        bool success = false;
        if (trainWarning != null)
        {
            _ = _context.TrainWarnings.Remove(trainWarning);
            success = await _context.SaveChangesAsync() > 0;
        }
        return success;
    }
}
