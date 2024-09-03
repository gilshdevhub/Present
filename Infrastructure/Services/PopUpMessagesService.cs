using Core.Entities.AppMessages;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PopUpMessagesService : IPopUpMessagesService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;

    public PopUpMessagesService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<PopUpMessages>> GetMessagesAsync()
    {
        IEnumerable<PopUpMessages> messages = await _context.PopUpMessages.Include(p => p.PageType).Include(p => p.SystemType).ToArrayAsync().ConfigureAwait(false);
        return messages;
    }
    public async Task<IEnumerable<PopUpMessages>> GetMessagesByStationAsync(int StationId)
    {
        IEnumerable<PopUpMessages> messages = await _context.PopUpMessages.Where(p => p.StationsIds.Contains(StationId.ToString())).ToArrayAsync().ConfigureAwait(false);
        return messages;
    }

    public async Task<IEnumerable<PopUpMessages>> GetMessagesAsync(MessageRequest messageRequest)
    {
        IEnumerable<PopUpMessages> popUpMessages = await _cacheService.GetAsync<IEnumerable<PopUpMessages>>(CacheKeys.PopUpMessages).ConfigureAwait(false);

        if (popUpMessages == null || popUpMessages.Count() <= 0)
        {
            popUpMessages = await _context.PopUpMessages.Where(p => p.isActive).ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<PopUpMessages>>(CacheKeys.PopUpMessages, popUpMessages).ConfigureAwait(false);
        }

        popUpMessages = popUpMessages.AsQueryable<PopUpMessages>()
               .Where(p => p.isActive && p.PageTypeId == messageRequest.PageTypeId && p.EndDate > DateTime.Now);
              
        switch (messageRequest.SystemTypeId.HasValue, messageRequest.StationId.HasValue)
        {
            case (true,true):
                popUpMessages = popUpMessages.Where(p =>
                            p.SystemTypeId == messageRequest.SystemTypeId && !string.IsNullOrEmpty(p.StationsIds)&& !string.IsNullOrWhiteSpace(p.StationsIds));
                              popUpMessages = popUpMessages.Where(p =>  p.StationsIds.Split(",").ToList().Contains(messageRequest.StationId.ToString()));
                break;
            case (true, false):
                popUpMessages = popUpMessages.Where(p => (
                                p.SystemTypeId == messageRequest.SystemTypeId
                            ));
                break;
            case (false, true):
                popUpMessages = popUpMessages.Where(p =>
                             !string.IsNullOrEmpty(p.StationsIds) && !string.IsNullOrWhiteSpace(p.StationsIds));
                popUpMessages = popUpMessages.Where(p => p.StationsIds.Split(",").ToList().Contains(messageRequest.StationId.ToString()));
                break;
            case (false, false):
            default:
                break;
        }

        return popUpMessages.ToArray();
    }

    private PopUpMessages ReplaceText(PopUpMessages message)
    {
        message.MessageBodyRussian = message.MessageBodyRussian.Replace("&nbsp;", " ");
        message.MessageBodyHebrew = message.MessageBodyHebrew.Replace("&nbsp;", " ");
        message.MessageBodyEnglish = message.MessageBodyEnglish.Replace("&nbsp;", " ");
        message.MessageBodyArabic = message.MessageBodyArabic.Replace("&nbsp;", " ");
        return message;
    }


    public async Task<IEnumerable<PageType>> GetPageTypesAsync()
    {
        return await _context.PageTypes.ToArrayAsync();
    }
    #region AddDeleteUpdate
    public async Task<bool> AddMessageAsync(PopUpMessages message)
    {
        message = ReplaceText(message);
        _ = await _context.PopUpMessages.AddAsync(message).ConfigureAwait(false);
        bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.PopUpMessages).ConfigureAwait(false);
            IEnumerable<PopUpMessages> popUpMessages = await _context.PopUpMessages.Where(p => p.isActive).ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<PopUpMessages>>(CacheKeys.PopUpMessages, popUpMessages).ConfigureAwait(false);
        }
        return success;
    }
    public async Task<bool> DeleteMessageAsync(int Id)
    {
        PopUpMessages message = await _context.PopUpMessages.SingleOrDefaultAsync(p => p.Id == Id);
        bool success = false;
        if (message != null)
        {
            _ = _context.PopUpMessages.Remove(message);
            success = await _context.SaveChangesAsync() > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.PopUpMessages).ConfigureAwait(false);
                IEnumerable<PopUpMessages> popUpMessages = await _context.PopUpMessages.Where(p => p.isActive).ToArrayAsync().ConfigureAwait(false);
                await _cacheService.SetAsync<IEnumerable<PopUpMessages>>(CacheKeys.PopUpMessages, popUpMessages).ConfigureAwait(false);
            }
        }
        return success;
    }
    public async Task<bool> DeleteMessageByStationAsync(int StationId)
    {
        IEnumerable<PopUpMessages> messages = await _context.PopUpMessages.Where(p => p.StationsIds.Contains(StationId.ToString())).ToArrayAsync().ConfigureAwait(false);
        bool success = false;
        if (messages != null)
        {
            foreach (var message in messages)
            {
                _ = _context.PopUpMessages.Remove(message);
            }
            success = await _context.SaveChangesAsync() > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.PopUpMessages).ConfigureAwait(false);
                IEnumerable<PopUpMessages> popUpMessages = await _context.PopUpMessages.Where(p => p.isActive).ToArrayAsync().ConfigureAwait(false);
                await _cacheService.SetAsync<IEnumerable<PopUpMessages>>(CacheKeys.PopUpMessages, popUpMessages).ConfigureAwait(false);
            }
        }
        return success;
    }
    public async Task<bool> UpdateMessageAsync(PopUpMessages messageToUpdate)
    {
        messageToUpdate = ReplaceText(messageToUpdate);
        _ = _context.PopUpMessages.Attach(messageToUpdate);
        _context.Entry(messageToUpdate).State = EntityState.Modified;

        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.PopUpMessages).ConfigureAwait(false);
            IEnumerable<PopUpMessages> popUpMessages = await _context.PopUpMessages.Where(p => p.isActive).ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<PopUpMessages>>(CacheKeys.PopUpMessages, popUpMessages).ConfigureAwait(false);
        }
        return success;
    }
    #endregion AddDeleteUpdate
}
