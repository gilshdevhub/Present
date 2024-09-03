using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Services;

public class SingleSupplierIService : ISingleSupplierIService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ITendersCommonService _tendersCommonService;
    public SingleSupplierIService(RailDbContext context, ICacheService cacheService, ITendersCommonService tendersCommonService)
    {
        _context = context;
        _cacheService = cacheService;
        _tendersCommonService = tendersCommonService;
    }

  

    public async Task<IEnumerable<SingleSupplierDto>> GetSuppliersAsync()
    {
        IEnumerable<SingleSupplierDto> singleSuppliers = await GetSingleSupplierContentAsync().ConfigureAwait(false);

        return singleSuppliers;
    }
    public async Task<IEnumerable<SingleSupplierDto>> GetSuppliersNoCache()
    {
        IEnumerable<SingleSupplierDto> allSingleSuppliers = await _context.SingleSupplier.Select(x => new SingleSupplierDto
        {
            Id = x.Id,
            PublishDate = x.PublishDate,
            DecisionDate = x.DecisionDate,
            SupplierName = x.SupplierName,
            ReferentName = x.ReferentName,
            ReferentMail = x.ReferentMail,
            Subject = x.Subject,
            UpdatingUser = x.UpdatingUser,
            Language = x.Language,
            Filed = x.Domain,
            Type = x.TypeOfTender,
            Category = x.Page,
            Documentation = _context.TenderDocuments
                                    .Where(y => y.SingleSupplierId == x.Id)
                                    .Select(x => new TenderDocumentsDto
                                    {
                                        DocDisplay = x.DocDisplay,
                                        DocName = x.DocName,
                                        DocType = x.DocType,
                                        Id = x.Id
                                    }).ToArray(),
            MailingList = x.MailingList
        })
                .ToArrayAsync().ConfigureAwait(false);
      


        return allSingleSuppliers;
    }
    public async Task<IEnumerable<SingleSupplierDto>> GetSuppliersAsync(int categoryId)
    {
        IEnumerable<SingleSupplierDto> singleSuppliers = (await GetSingleSupplierContentAsync().ConfigureAwait(false)).Where(x => x.Category == categoryId);

        return singleSuppliers;
    }
    public async Task<IEnumerable<SingleSupplierDto>> GetSuppliersNoCache(int categoryId)
    {
        IEnumerable<SingleSupplierDto> singleSuppliers = (await GetSuppliersNoCache().ConfigureAwait(false)).Where(x => x.Category == categoryId);

        return singleSuppliers;
    }
    public async Task<SingleSupplierDto> GetSupplierByIdAsync(Guid Id)
    {
        SingleSupplierDto singleSupplier = (await GetSingleSupplierContentAsync().ConfigureAwait(false)).SingleOrDefault(x => x.Id == Id);

        return singleSupplier;
    }
    
    private async Task<IEnumerable<SingleSupplierDto>> GetSingleSupplierContentAsync()
    {
        _ = await _tendersCommonService.GetTenderDocumentsContentAsync().ConfigureAwait(false);
        IEnumerable<SingleSupplierDto> allSingleSuppliers = await _cacheService.GetAsync<IEnumerable<SingleSupplierDto>>(CacheKeys.SingleSupplier).ConfigureAwait(false);

        if (allSingleSuppliers == null || allSingleSuppliers.Count() <= 0)
        {
            allSingleSuppliers = await UpdateCacheSingleSupplierAsync();
        }

        return allSingleSuppliers;
    }
    public async Task<IEnumerable<SingleSupplierDto>> UpdateCacheSingleSupplierAsync()
    {
        IEnumerable<SingleSupplierDto> allSingleSuppliers = await GetSuppliersNoCache();
        await _cacheService.SetAsync(CacheKeys.SingleSupplier, allSingleSuppliers).ConfigureAwait(false);

        return allSingleSuppliers;
    }

    #region AddDeleteUpdate

    public async Task<SingleSupplier> AddSupplierAsync(SingleSupplier supplier)
    {
        SingleSupplier suppliers = await _context.SingleSupplier.Where(i => i.Id == supplier.Id).SingleOrDefaultAsync();

        if (suppliers != null)
        {
            return null;
        }
        EntityEntry<SingleSupplier> entity = _context.SingleSupplier.Add(supplier);
        SingleSupplier newSupplier = entity.Entity;

        bool success = await _context.SaveChangesAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.SingleSupplier);
            _ = await UpdateCacheSingleSupplierAsync();
            return newSupplier;
        }
        return newSupplier;
    }
    public async Task<bool> DeleteSupplierAsync(Guid Id)
    {
        bool success;
        SingleSupplier item = await _context.SingleSupplier.SingleOrDefaultAsync(p => p.Id == Id);
        if (item != null)
        {
            IEnumerable<TenderDocuments> docs = _context.TenderDocuments.Where(x => x.SingleSupplierId == Id);
            if (docs != null)
            {
                _context.TenderDocuments.RemoveRange(docs.First(), docs.Last());
            }
        }
        _ = _context.SingleSupplier.Remove(item);
        try
        {
             success = await _context.SaveChangesAsync() > 0;
        } catch (Exception ex) 
        { throw ex; 
        }
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(CacheKeys.SingleSupplier);
            _ = await UpdateCacheSingleSupplierAsync();
        }
        return success;
    }
    public async Task<SingleSupplier> UpdateSupplierAsync(SingleSupplier supplierToUpdate)
    {

        SingleSupplier suppliers = await _context.SingleSupplier.SingleOrDefaultAsync(p => p.Id == supplierToUpdate.Id);

        if (suppliers != null)
        {
            _ = _context.SingleSupplier.Attach(suppliers);
            _ = _context.SingleSupplier.Remove(suppliers);

            EntityEntry<SingleSupplier> entity = _context.SingleSupplier.Attach(supplierToUpdate);

            _context.Entry(supplierToUpdate).State = EntityState.Modified;

            if (await _context.SaveChangesAsync().ConfigureAwait(false) > 0)
            {
                await _cacheService.RemoveCacheItemAsync(CacheKeys.SingleSupplier);
                suppliers = entity.Entity;
                _ = await UpdateCacheSingleSupplierAsync();
            }
        }

        return suppliers;

    }
   
    #endregion AddDeleteUpdate

}
