using Core.Entities;

namespace Core.Interfaces;

public interface IMeetingsService
{
    Task<IEnumerable<MeetingsDto>> GetMeetingsAsync();
    Task<IEnumerable<MeetingsDto>> GetMeetingsNoCache();
    Task<MeetingsDto?> GetMeetingByIdAsync(int Id);
    Task<MeetingsDto?> GetMeetingByIdNoCache(int Id);
    Task<Meetings?> AddMeetingsAsync(Meetings tender);
    Task<bool> AddMeetingsAsync(IEnumerable<Meetings> meetings);
    Task<bool> UpdateMeetingsAsync(Meetings tendersToUpdate);
    Task<bool> DeleteMeetingsAsync(int Id);
    Task<IEnumerable<Meetings>> GetMeetingsContentAsync();
}
