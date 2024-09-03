using Core.Entities;
using Core.Enums;
using Newtonsoft.Json.Linq;

namespace Core.Interfaces;

public interface IURLTranslationService
{
    Task<IEnumerable<URLTranslation>> GetAllURLTranslationsAsync();
    Task<URLTranslation> GetItemAsync(string key, SystemTypes systemType);
    Task<bool> DeleteAsync(int id);
    Task<URLTranslation> GetItemByIdAsync(int id);
    Task<URLTranslation> AddURLTranslationAsync(URLTranslation item);
    Task<bool> UpdateURLTranslationAsync(URLTranslation item);
    Task<IEnumerable<URLTranslation>> GetActiveURLTranslationsAsync(SystemTypes systemType);
    Task<JObject> GetActiveURLTranslations1Async(URLTranslationRequest translationRequest);
    Task<JObject> GetActiveURLTranslations2Async(URLTranslationRequest translationRequest);
}
