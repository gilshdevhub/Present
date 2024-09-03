using Core.Entities;
using Core.Entities.Configuration;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services;

public class URLTranslationService : IURLTranslationService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ILogger<URLTranslationService> _logger;
    private readonly IMailService _mailService;

    public URLTranslationService(RailDbContext context, ICacheService cacheService, ILogger<URLTranslationService> logger, IMailService mailService)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
        _mailService = mailService;
    }

    public async Task<IEnumerable<URLTranslation>> GetAllURLTranslationsAsync()
    {
        return await GetURLTranslationsAllAsync().ConfigureAwait(false);
    }

                  
    public async Task<URLTranslation> GetItemByIdAsync(int id)
    {
        URLTranslation urlTranslation = await _context.URLTranslations.Include(p => p.SystemType).SingleOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
        return urlTranslation;
    }

    public async Task<URLTranslation> GetItemAsync(string key, SystemTypes systemType)
    {
        URLTranslation urlTranslation = await _context.URLTranslations
            .Include(p => p.SystemType)
            .SingleOrDefaultAsync(p => p.Key == key && p.SystemType.Id == (int)systemType);
        return urlTranslation;
    }


    private Task<int> CompleteAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<URLTranslation>> GetActiveURLTranslationsAsync(SystemTypes systemType)
    {
        return await _context.URLTranslations.Where(p => p.IsActive && p.SystemTypeId == (int)systemType)
            .ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<JObject> GetActiveURLTranslations1Async(URLTranslationRequest urlTranslationRequest)
    {
        IEnumerable<URLTranslation> urlTranslations = await GetURLTranslationsAsync((int)urlTranslationRequest.SystemType).ConfigureAwait(false);

        const string json = @"{""he"": {}, ""en"": {}, ""ru"": {}, ""ar"": {} }";

        JObject jo = JObject.Parse(json);
        JObject he = jo["he"] as JObject;
        JObject en = jo["en"] as JObject;
        JObject ar = jo["ar"] as JObject;
        JObject ru = jo["ru"] as JObject;

        foreach (URLTranslation urlTranslation in urlTranslations)
        {
            try
            {
                he.Add(urlTranslation.Key, urlTranslation.Hebrew);
                en.Add(urlTranslation.Key, urlTranslation.English);
                ar.Add(urlTranslation.Key, urlTranslation.Arabic);
                ru.Add(urlTranslation.Key, urlTranslation.Russian);
            }
            catch (Exception ex)
            {
                _logger.LogError($"error in urlTranslation with key: {urlTranslation.Key}", ex);
                           }
        }

        return jo;
    }

    public async Task<JObject> GetActiveURLTranslations2Async(URLTranslationRequest urlTranslationRequest)
    {
        IEnumerable<URLTranslation> urlTranslations = await GetURLTranslationsAsync((int)urlTranslationRequest.SystemType).ConfigureAwait(false);

        string json;

        JObject jo = null;

        switch (urlTranslationRequest.LanguageId)
        {
            case Languages.Hebrew:
                json = @"{""he"": {} }";
                jo = JObject.Parse(json);
                JObject he = jo["he"] as JObject;
                foreach (URLTranslation urlTranslation in urlTranslations)
                {
                    he.Add(urlTranslation.Key, urlTranslation.Hebrew);
                }
                break;
            case Languages.English:
                json = @"{""en"": {} }";
                jo = JObject.Parse(json);
                JObject en = jo["en"] as JObject;
                foreach (URLTranslation urlTranslation in urlTranslations)
                {
                    en.Add(urlTranslation.Key, urlTranslation.English);
                }
                break;
            case Languages.Arabic:
                json = @"{""ar"": {} }";
                jo = JObject.Parse(json);
                JObject ar = jo["ar"] as JObject;
                foreach (URLTranslation urlTranslation in urlTranslations)
                {
                    ar.Add(urlTranslation.Key, urlTranslation.Arabic);
                }
                break;
            case Languages.Russian:
                json = @"{""ru"": {} }";
                jo = JObject.Parse(json);
                JObject ru = jo["ru"] as JObject;
                foreach (URLTranslation urlTranslation in urlTranslations)
                {
                    ru.Add(urlTranslation.Key, urlTranslation.Russian);
                }
                break;
        }

        return jo;
    }

    private async Task<IEnumerable<URLTranslation>> GetURLTranslationsAsync(int systemType)
    {
        IEnumerable<URLTranslation> urlTranslations = await _cacheService.GetAsync<IEnumerable<URLTranslation>>(CacheKeys.URLTranslations);

        if (urlTranslations == null || urlTranslations.Count() <= 0)
        {
            urlTranslations = await _context.URLTranslations.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<URLTranslation>>(CacheKeys.URLTranslations, urlTranslations).ConfigureAwait(false);
        }

        return urlTranslations.Where(p => p.IsActive && p.SystemTypeId == systemType);
    }

    private async Task<IEnumerable<URLTranslation>> GetURLTranslationsAllAsync()
    {
        IEnumerable<URLTranslation> urlTranslations = await _context.URLTranslations.ToArrayAsync().ConfigureAwait(false);
     
        return urlTranslations;
    }
    #region AddDeleteUpdate
    public async Task<URLTranslation> AddURLTranslationAsync(URLTranslation item)
    {

        URLTranslation configuration = await _context.URLTranslations
           .Where(i => (i.Key == item.Key && i.SystemTypeId == item.SystemTypeId)).SingleOrDefaultAsync();

        if (configuration != null)
        {
            return null;
        }
        var entity = _context.URLTranslations.Add(item);
        URLTranslation urlTranslation = entity.Entity;

        bool success = await CompleteAsync() > 0;

        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.URLTranslations);
            IEnumerable<URLTranslation> urlTranslations = await _context.URLTranslations.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<URLTranslation>>(CacheKeys.URLTranslations, urlTranslations).ConfigureAwait(false);
            return urlTranslation;
        }
        else
        {
            return null;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        URLTranslation urlTranslation = await GetItemByIdAsync(id);
        bool success = false;
        if (urlTranslation != null)
        {
            _ = _context.URLTranslations.Remove(urlTranslation);
            success = await _context.SaveChangesAsync() > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.URLTranslations);
                IEnumerable<URLTranslation> urlTranslations = await _context.URLTranslations.ToArrayAsync().ConfigureAwait(false);
                await _cacheService.SetAsync<IEnumerable<URLTranslation>>(CacheKeys.URLTranslations, urlTranslations).ConfigureAwait(false);
            }
        }

        return success;
    }

    public async Task<bool> UpdateURLTranslationAsync(URLTranslation item)
    {
        _ = _context.URLTranslations.Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        bool success = await CompleteAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.URLTranslations);
            IEnumerable<URLTranslation> urlTranslations = await _context.URLTranslations.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<URLTranslation>>(CacheKeys.URLTranslations, urlTranslations).ConfigureAwait(false);
        }
        return success;
    }
    #endregion AddDeleteUpdate
}
