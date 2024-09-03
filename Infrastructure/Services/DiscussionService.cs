using Core.Entities.SpeakerTimer;
using Core.Entities.Translation;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Infrastructure.Services;

public class DiscussionService : IDiscussionService
{
    private readonly RailDbContext _context;

    public DiscussionService(RailDbContext context)
    {
        _context = context;
    }

   
    public async Task<IEnumerable<Discussion>> GetAllDiscussionsAsync()
    {
        return await _context.Discussion.ToListAsync();
    }
    public async Task<Discussion> GetDiscussionByIdAsync(int id)
    {
        return await _context.Discussion.Where(x => x.Id == id).FirstOrDefaultAsync();
    }
    public async Task<bool> DeleteDiscussionByIdAsync(int id)
    {
        Discussion item = await _context.Discussion.Where(x => x.Id == id).FirstOrDefaultAsync();
        bool success = false;
        if (item != null)
        {
            _ = _context.Discussion.Remove(item);
            success = await _context.SaveChangesAsync() > 0;
          
        }

        return success;
    }
    public async Task<IEnumerable<Discussion>> AddDiscussionAsync(Discussion item)
    {
        var entity = _context.Discussion.Add(item);
        Discussion discussion = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0; 

        if (success)
        {
            IEnumerable<Discussion> discussions = await _context.Discussion
                .ToArrayAsync().ConfigureAwait(false);
            return discussions;
        }
        else
        {
            return null;
        }
    }

    public async Task<IEnumerable<Discussion>> UpdateDiscussionAsync(Discussion item)
    {
        IEnumerable<Discussion> Discussion = null;
        _ = _context.Discussion.Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            Discussion = await _context.Discussion.ToArrayAsync().ConfigureAwait(false);
        }
        return Discussion;
    }
}
