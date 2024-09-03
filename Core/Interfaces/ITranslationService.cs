using Core.Entities.Translation;
using Core.Enums;
using Newtonsoft.Json.Linq;

namespace Core.Interfaces;

public interface ITranslationService
{
    Task<IEnumerable<Translation>> GetAllTranslationsAsync();
    Task<Translation> GetItemAsync(string key, SystemTypes systemType);
    Task<bool> DeleteAsync(int id);
    Task<Translation> GetItemByIdAsync(int id);
    Task<Translation> AddTranslationAsync(Translation item);
    Task<bool> UpdateTranslationAsync(Translation item);
    Task<IEnumerable<Translation>> GetActiveTranslationsAsync(SystemTypes systemType);
    Task<JObject> GetActiveTranslations1Async(TranslationRequest translationRequest);
    Task<JObject> GetActiveTranslations2Async(TranslationRequest translationRequest);
}
