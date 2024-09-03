using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class MailingListService : IMailingListService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;

    public MailingListService(RailDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }
 
    public async Task<IEnumerable<MailingList>> GetMailingListAsync()
    {
        IEnumerable<MailingList> mailingLists = await GetMailingListsContentAsync().ConfigureAwait(false);

        return mailingLists;
    }
    public async Task<MailingList?> GetMailingListByIdAsync(int Id)
    {
        MailingList? mailingList = (await GetMailingListsContentAsync().ConfigureAwait(false)).SingleOrDefault(x => x.MailingListId == Id);

        return mailingList;
    }
    private async Task<IEnumerable<MailingList>> GetMailingListsContentAsync()
    {
        IEnumerable<MailingList> allMailingLists = await _cacheService.GetAsync<IEnumerable<MailingList>>(CacheKeys.MailingList).ConfigureAwait(false);
        if (allMailingLists == null || allMailingLists.Count() <= 0)
        {
            allMailingLists = await _context.MailingList.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync(CacheKeys.MailingList, allMailingLists).ConfigureAwait(false);
        }

        return allMailingLists;
    }
    public async Task<IEnumerable<MailingList>> GetMailingListsContentNoCache()
    {
        IEnumerable<MailingList> allMailingLists =  await _context.MailingList.ToArrayAsync().ConfigureAwait(false);
        
        return allMailingLists;
    }
    public async Task<MailingList> GetMailingListByIdNoCache(int Id)
    {
        MailingList mailingList = (await GetMailingListsContentNoCache().ConfigureAwait(false)).SingleOrDefault(x => x.MailingListId == Id);

        return mailingList;
    }

    #region AddDeleteUpdate
    public async Task<MailingList> AddMailingListAsync(MailingList mailingList)
    {
        MailingList mailingLists = (await GetMailingListsContentAsync().ConfigureAwait(false))
           .Where(i => i.MailingListId == mailingList.MailingListId).SingleOrDefault();

        if (mailingLists != null)
        {
            return null;
        }
        _ = mailingList.Mails.Replace(" ", ",").Replace(";", ",");
        EntityEntry<MailingList> entity = _context.MailingList.Add(mailingList);
        MailingList newMailingList = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {

            IEnumerable<MailingList> allMailingLists = await _context.MailingList.ToArrayAsync().ConfigureAwait(false);

            await _cacheService.RemoveCacheItemAsync(CacheKeys.MailingList);
            await _cacheService.SetAsync(CacheKeys.MailingList, allMailingLists).ConfigureAwait(false);
            return newMailingList;
        }
        return newMailingList;
    }
    public async Task<bool> DeleteMailingListAsync(int Id)
    {
        MailingList item = (await GetMailingListsContentAsync().ConfigureAwait(false)).SingleOrDefault(p => p.MailingListId == Id);
        _ = _context.MailingList.Remove(item);
        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {

            IEnumerable<MailingList> allMailingLists = await _context.MailingList.ToArrayAsync().ConfigureAwait(false);

            await _cacheService.RemoveCacheItemAsync(CacheKeys.MailingList);
            await _cacheService.SetAsync(CacheKeys.MailingList, allMailingLists).ConfigureAwait(false);
        }
        return success;
    }
    public async Task<bool> DeleteSingleMailAsync(string mail)
    {
        IEnumerable<MailingList> mailingLists = (await GetMailingListsContentAsync().ConfigureAwait(false)).Where(m => m.Mails.Contains(mail));
        List<bool> success = new();
        if (mailingLists != null)
        {
            IEnumerable<MailingList> listForDelete = await _context.MailingList
                                        .Where(p => p.Mails == mail).ToListAsync().ConfigureAwait(false);

            foreach (MailingList row in listForDelete)
            {
                _ = _context.MailingList.Remove(row);

                if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
                {
                    IEnumerable<MailingList> allMailingLists = await _context.MailingList.ToArrayAsync().ConfigureAwait(false);
                    await _cacheService.RemoveCacheItemAsync(CacheKeys.MailingList);
                    await _cacheService.SetAsync(CacheKeys.MailingList, allMailingLists).ConfigureAwait(false);
                    success.Add(true);
                }
                else
                {
                    success.Add(false);
                    break;
                }
            }

        }
        if (success.All(x => x == true))
            return true;
        else
            return false;
    }
    public async Task<MailingList> UpdateMailingListAsync(MailingList mailingListToUpdate)
    {
        MailingList mailingLists = (await GetMailingListsContentAsync().ConfigureAwait(false)).SingleOrDefault(p => p.MailingListId == mailingListToUpdate.MailingListId);
        string oldlist = mailingLists.Mails;
        if (mailingLists != null)
        {
            mailingListToUpdate.Mails += "," + oldlist;
            mailingListToUpdate.Name = mailingLists.Name;
            _ = _context.MailingList.Attach(mailingLists);
            _ = _context.MailingList.Remove(mailingLists);

            EntityEntry<MailingList> entity = _context.MailingList.Attach(mailingListToUpdate);

            _context.Entry(mailingListToUpdate).State = EntityState.Modified;

            if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
            {

                IEnumerable<MailingList> allMailingLists = await _context.MailingList.ToArrayAsync().ConfigureAwait(false);

                await _cacheService.RemoveCacheItemAsync(CacheKeys.MailingList);
                await _cacheService.SetAsync(CacheKeys.MailingList, allMailingLists).ConfigureAwait(false);
                mailingLists = entity.Entity;
            }
        }

        return mailingLists;
    }
    #endregion AddDeleteUpdate
}
