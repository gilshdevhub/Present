using Core.Entities.Translation;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services;

public class TranslationService : ITranslationService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ILogger<TranslationService> _logger;
    private readonly IMailService _mailService;

    public TranslationService(RailDbContext context, ICacheService cacheService, ILogger<TranslationService> logger, IMailService mailService)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
        _mailService = mailService;
    }

    public async Task<IEnumerable<Translation>> GetAllTranslationsAsync()
    {
        return await GetTranslationsAllAsync().ConfigureAwait(false);
    }

                  
    public async Task<Translation> GetItemByIdAsync(int id)
    {
        Translation translation = await _context.Translations.Include(p => p.SystemType).SingleOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
        return translation;
    }

    public async Task<Translation> GetItemAsync(string key, SystemTypes systemType)
    {
        Translation translation = await _context.Translations
            .Include(p => p.SystemType)
            .SingleOrDefaultAsync(p => p.Key == key && p.SystemType.Id == (int)systemType);
        return translation;
    }

    private Task<int> CompleteAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Translation>> GetActiveTranslationsAsync(SystemTypes systemType)
    {
        return await _context.Translations.Where(p => p.IsActive && p.SystemTypeId == (int)systemType)
            .ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<JObject> GetActiveTranslations1Async(TranslationRequest translationRequest)
    {
        IEnumerable<Translation> translations;
        try
        {
            translations = await GetTranslationsAsync((int)translationRequest.SystemType).ConfigureAwait(false);

            const string json = @"{""he"": {}, ""en"": {}, ""ru"": {}, ""ar"": {} }";

            JObject jo = JObject.Parse(json);
            JObject he = jo["he"] as JObject;
            JObject en = jo["en"] as JObject;
            JObject ar = jo["ar"] as JObject;
            JObject ru = jo["ru"] as JObject;

            foreach (Translation translation in translations)
            {
                try
                {
                    he.Add(translation.Key, translation.Hebrew);
                    en.Add(translation.Key, translation.English);
                    ar.Add(translation.Key, translation.Arabic);
                    ru.Add(translation.Key, translation.Russian);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"error in translation with key: {translation.Key}", ex);
                                   }
            }
            return jo;
        }
        catch (Exception ex)
        {
            _logger.LogError($"error in translation ", ex);
            throw new Exception(ex.Message, ex);
        }


    }

    public async Task<JObject> GetActiveTranslations2Async(TranslationRequest translationRequest)
    {
        IEnumerable<Translation> translations = await GetTranslationsAsync((int)translationRequest.SystemType).ConfigureAwait(false);

        string json;

        JObject jo = null;

        switch (translationRequest.LanguageId)
        {
            case Languages.Hebrew:
                json = @"{""he"": {} }";
                jo = JObject.Parse(json);
                JObject he = jo["he"] as JObject;
                foreach (Translation translation in translations)
                {
                    he.Add(translation.Key, translation.Hebrew);
                }
                break;
            case Languages.English:
                json = @"{""en"": {} }";
                jo = JObject.Parse(json);
                JObject en = jo["en"] as JObject;
                foreach (Translation translation in translations)
                {
                    en.Add(translation.Key, translation.English);
                }
                break;
            case Languages.Arabic:
                json = @"{""ar"": {} }";
                jo = JObject.Parse(json);
                JObject ar = jo["ar"] as JObject;
                foreach (Translation translation in translations)
                {
                    ar.Add(translation.Key, translation.Arabic);
                }
                break;
            case Languages.Russian:
                json = @"{""ru"": {} }";
                jo = JObject.Parse(json);
                JObject ru = jo["ru"] as JObject;
                foreach (Translation translation in translations)
                {
                    ru.Add(translation.Key, translation.Russian);
                }
                break;
        }

        return jo;
    }

    private async Task<IEnumerable<Translation>> GetTranslationsAsync(int systemType)
    {
        IEnumerable<Translation> translations = await _cacheService.GetAsync<IEnumerable<Translation>>(CacheKeys.Translations);

        if (translations == null || translations.Count() <= 0)
        {
            translations = await _context.Translations.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<Translation>>(CacheKeys.Translations, translations).ConfigureAwait(false);
        }

        return translations.Where(p => p.IsActive && p.SystemTypeId == systemType);
    }

    private async Task<IEnumerable<Translation>> GetTranslationsAllAsync()
    {
        IEnumerable<Translation> translations = await _context.Translations.ToArrayAsync().ConfigureAwait(false);

        return translations;
    }
    #region AddDeleteUpdate
    public async Task<Translation> AddTranslationAsync(Translation item)
    {
        var entity = _context.Translations.Add(item);
        Translation translation = entity.Entity;

        bool success = await CompleteAsync() > 0;

        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.Translations);
            IEnumerable<Translation> translations = await _context.Translations.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<Translation>>(CacheKeys.Translations, translations).ConfigureAwait(false);
            return translation;
        }
        else
        {
            return null;
        }
    }
    public async Task<bool> DeleteAsync(int id)
    {
        Translation translation = await GetItemByIdAsync(id);
        bool success = false;
        if (translation != null)
        {
            _ = _context.Translations.Remove(translation);
            success = await _context.SaveChangesAsync() > 0;
            if (success)
            {
                await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.Translations);
                IEnumerable<Translation> translations = await _context.Translations.ToArrayAsync().ConfigureAwait(false);
                await _cacheService.SetAsync<IEnumerable<Translation>>(CacheKeys.Translations, translations).ConfigureAwait(false);
            }
        }

        return success;
    }
    public async Task<bool> UpdateTranslationAsync(Translation item)
    {
        _ = _context.Translations.Attach(item);
        _context.Entry(item).State = EntityState.Modified;
        bool success = await CompleteAsync() > 0;
        if (success)
        {
            await _cacheService.RemoveCacheItemAsync(Core.Enums.CacheKeys.Translations);
            IEnumerable<Translation> translations = await _context.Translations.ToArrayAsync().ConfigureAwait(false);
            await _cacheService.SetAsync<IEnumerable<Translation>>(CacheKeys.Translations, translations).ConfigureAwait(false);
        }
        return success;
    }
    #endregion AddDeleteUpdate
}
