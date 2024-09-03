using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class MeetingsService : IMeetingsService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ITendersService _tendersService;
    public MeetingsService(ITendersService tendersService, RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
        _tendersService = tendersService;
    }

    public async Task<IEnumerable<MeetingsDto>> GetMeetingsAsync()
    {
        IEnumerable<MeetingsDto> meetings = (await GetMeetingsContentAsync().ConfigureAwait(false)).Select(x => new MeetingsDto
        {
            MeetingsId = x.MeetingsId,
            RegistretionLink = x.RegistretionLink,
            MeetingDate = x.MeetingDate,
            Location = x.Location,
        });

        return meetings;
    }

    public async Task<IEnumerable<MeetingsDto>> GetMeetingsNoCache()
    {

        return await _context.Meetings.Select(x => new MeetingsDto
        {
            MeetingsId = x.MeetingsId,
            RegistretionLink = x.RegistretionLink,
            MeetingDate = x.MeetingDate,
            Location = x.Location,
        }).ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<MeetingsDto?> GetMeetingByIdAsync(int Id)
    {
        MeetingsDto? meeting = (await GetMeetingsContentAsync().ConfigureAwait(false)).Select(x => new MeetingsDto
        {
            MeetingsId = x.MeetingsId,
            RegistretionLink = x.RegistretionLink,
            MeetingDate = x.MeetingDate,
            Location = x.Location,
        }).SingleOrDefault(x => x.MeetingsId == Id);

        return meeting;
    }

    public async Task<MeetingsDto?> GetMeetingByIdNoCache(int Id)
    {

        return await _context.Meetings.Select(x => new MeetingsDto
        {
            MeetingsId = x.MeetingsId,
            RegistretionLink = x.RegistretionLink,
            MeetingDate = x.MeetingDate,
            Location = x.Location,
        }).SingleOrDefaultAsync(x => x.MeetingsId == Id).ConfigureAwait(false);
       
    }
   
    public async Task<IEnumerable<Meetings>> GetMeetingsContentAsync()
    {
        IEnumerable<Meetings> allMeetings = await _cacheService.GetAsync<IEnumerable<Meetings>>(CacheKeys.Meetings).ConfigureAwait(false);
        if (allMeetings == null || allMeetings.Count() <= 0)
        {
            allMeetings = await _context.Meetings.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.Meetings, allMeetings).ConfigureAwait(false);
        }

        return allMeetings;
    }
    #region AddDeleteUpdate
    public async Task<Meetings?> AddMeetingsAsync(Meetings meeting)
    {
        MeetingsDto? meetings = (await GetMeetingsContentAsync().ConfigureAwait(false)).Select(x => new MeetingsDto
        {
            MeetingsId = x.MeetingsId,
            RegistretionLink = x.RegistretionLink,
            MeetingDate = x.MeetingDate,
            Location = x.Location,
                   })
            .Where(i => i.MeetingsId == meeting.MeetingsId).SingleOrDefault();

        if (meetings != null)
        {
            return null;
        }

        EntityEntry<Meetings> entity = _context.Meetings.Add(meeting);
        Meetings newMeeting = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {

            IEnumerable<Meetings> allMeetings = await _context.Meetings.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(CacheKeys.Meetings);
            await _cacheService.SetAsync(CacheKeys.Meetings, allMeetings).ConfigureAwait(false);
           
            _ = await _tendersService.UpdateCacheTendersAsync();
            return newMeeting;
        }
        return newMeeting;
    }
    public async Task<bool> AddMeetingsAsync(IEnumerable<Meetings> meetings)
    {

        await _context.Meetings.AddRangeAsync(meetings);
       
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            IEnumerable<Meetings> allMeetings = await _context.Meetings.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(CacheKeys.Meetings);
            await _cacheService.SetAsync(CacheKeys.Meetings, allMeetings).ConfigureAwait(false);

            _ = await _tendersService.UpdateCacheTendersAsync();
            return true;
        }
        return false;
    }
    public async Task<bool> DeleteMeetingsAsync(int Id)
    {
        Meetings? item = _context.Meetings.SingleOrDefault(p => p.MeetingsId == Id);
        _ = _context.Meetings.Remove(item);
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            IEnumerable<Meetings> allMeetings = await _context.Meetings.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(CacheKeys.Meetings);
            await _cacheService.SetAsync(CacheKeys.Meetings, allMeetings).ConfigureAwait(false);

            _ = await _tendersService.UpdateCacheTendersAsync();

        }
        return success;
    }
    public async Task<bool> UpdateMeetingsAsync(Meetings meetingToUpdate)
    {
        _ = _context.Meetings.Update(meetingToUpdate);

        if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
        {
            IEnumerable<Meetings> allMeetings = await _context.Meetings.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.RemoveCacheItemAsync(CacheKeys.Meetings);
            await _cacheService.SetAsync(CacheKeys.Meetings, allMeetings).ConfigureAwait(false);
            _ = await _tendersService.UpdateCacheTendersAsync();
        }

        return true;
    }
    #endregion AddDeleteUpdate
}
