using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class TendersService : ITendersService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ITendersCommonService _tendersCommonService;
    public TendersService(RailDbContext context, ICacheService cacheService,
        ITendersCommonService tendersCommonService)
    {
        _context = context;
        _cacheService = cacheService;
        _tendersCommonService = tendersCommonService;
    }

    public async Task<IEnumerable<TendersDto>> GetTendersAsync()
    {
        IEnumerable<TendersDto> tenders = await GetTendersContentAsync().ConfigureAwait(false);

        return tenders;
    }

    public async Task<IEnumerable<TendersDto>> GetTendersAsync(int categoryId)
    {
        IEnumerable<TendersDto> tenders = (await GetTendersContentAsync().ConfigureAwait(false)).Where(x => x.Category == categoryId);

        return tenders;
    }

    public async Task<IEnumerable<TendersDto>> GetTendersByTypeAsync(int type)
    {
        IEnumerable<TendersDto> tenders = await GetTendersContentAsync().ConfigureAwait(false);
        IEnumerable<TendersDto> result = tenders;
        try
        {
            result = type switch
            {
                1 => tenders.Where(t => (t.WinningSupplier == null || t.WinningSupplier == "")
                                        && t.BiddingDate > DateTime.UtcNow),
                2 => tenders.Where(t => (t.WinningSupplier == null || t.WinningSupplier == "")
                                        && t.BiddingDate < DateTime.UtcNow),
                3 => tenders.Where(t => t.WinningSupplier != null &&
                                             t.BiddingDate < DateTime.UtcNow
                                           ),
                10 => tenders,
                _ => tenders,
            };
        }
        catch (Exception)
        {
            throw;
        }
        return result;
    }

    public async Task<TendersDto> GetTenderByIdAsync(Guid Id)
    {
        TendersDto tenders = (await GetTendersContentAsync().ConfigureAwait(false)).SingleOrDefault(x => x.Id == Id);

        return tenders;
    }
    private async Task<IEnumerable<TendersDto>> GetTendersContentAsync()
    {
        _ = await _tendersCommonService.GetTenderDocumentsContentAsync().ConfigureAwait(false);
        IEnumerable<TendersDto> allTenders;
        allTenders = await UpdateCacheTendersAsync();
        return allTenders;
    }

    public async Task<IEnumerable<TendersDto>> UpdateCacheTendersAsync()
    {
        IEnumerable<TendersDto> allTenders = await _context.Tenders.Select(x => new TendersDto
        {
            Id = x.Id,
            PublishDate = x.PublishDate,
            ReferentName = x.ReferentName,
            ReferentMail = x.ReferentMail,
            Filed = x.Domain,
            Type = x.TypeOfTender,
            Category = x.Page,
            TenderNumber = x.TenderNumber,
            TenderName = x.TenderName,
            ClarifyingDate = x.ClarifyingDate,
            BiddingDate = x.BiddingDate,
            WinningSupplier = x.WinningSupplier,
            WinningDate = x.WinningDate,
            Meetings = _context.Meetings
                .Where(y => y.TendersId == x.Id)
                .Select(y => new MeetingsDto
                {
                    Location = y.Location,
                    MeetingDate = y.MeetingDate,
                    MeetingsId = y.MeetingsId,
                    RegistretionLink = y.RegistretionLink
                })
                .ToList(),
            Documentation = _context.TenderDocuments
                       .Where(q => q.TendersId == x.Id)
                       .Select(z => new TenderDocumentsDto
                       {
                           DocDisplay = z.DocDisplay,
                           DocName = z.DocName,
                           DocType = z.DocType,
                           Id = z.Id
                       }).ToList(),
            Description = x.Description,
            Biddings = x.Biddings,
            Language = x.Language,
            UpdateDate = x.UpdateDate,
            UpdatingUser = x.UpdatingUser,
            WaitingSupplier = x.WaitingSupplier,
            WinningAmount = x.WinningAmount,
                 })
        .ToArrayAsync().ConfigureAwait(false);
        await _cacheService.RemoveCacheItemAsync(CacheKeys.Tender).ConfigureAwait(false);
        await _cacheService.SetAsync(CacheKeys.Tender, allTenders).ConfigureAwait(false);
        return allTenders;
    }
    #region AddDeleteUpdate
    public async Task<Tenders> AddTendersAsync(Tenders tender)
    {
        TendersDto tenders = (await GetTendersContentAsync().ConfigureAwait(false))
            .Where(i => i.Id == tender.Id).SingleOrDefault();

        if (tenders != null)
        {
            return null;
        }
        EntityEntry<Tenders> entity = await _context.Tenders.AddAsync(tender).ConfigureAwait(false);

        Tenders newTender = entity.Entity;
        try
        {
            bool success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                               _ = await UpdateCacheTendersAsync();
                return newTender;
            }
        }
        catch (Exception)
        {
            throw;
        }
        return newTender;
    }
    public async Task<bool> DeleteTendersAsync(Guid Id)
    {
        bool success = false;
        Tenders item = await _context.Tenders
                                        .Include(p => p.Documentation)
                                        .Where(p => p.Id == Id).SingleOrDefaultAsync().ConfigureAwait(false);
        if (item != null)
        {
            _ = _context.Tenders.Remove(item);
            success = await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            if (success)
            {
                _ = await UpdateCacheTendersAsync();
            }

        }
        return success;
    }
    public async Task<Tenders> UpdateTendersAsync(Tenders tendersToUpdate)
    {
        Tenders tender = await _context.Tenders.Where(p => p.Id == tendersToUpdate.Id).SingleOrDefaultAsync().ConfigureAwait(false);

        if (tender != null)
        {
            _ = _context.Tenders.Attach(tender);
            _ = _context.Tenders.Remove(tender);

            EntityEntry<Tenders> entity = _context.Tenders.Attach(tendersToUpdate);

            _context.Entry(tendersToUpdate).State = EntityState.Modified;

            if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
            {
                               tender = entity.Entity;
                _ = await UpdateCacheTendersAsync();
            }
        }
        return tender;
    }
    public async Task<bool> TenderNumberExist(int Id)
    {
        IEnumerable<Tenders> allTender = [.. _context.Tenders.Where(x => x.TenderNumber==Id)];
        if (allTender==null || !allTender.Any())
        {
            return false;
        }
        return true;
    }
    #endregion AddDeleteUpdate
}
