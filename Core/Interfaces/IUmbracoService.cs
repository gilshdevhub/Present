using Core.Entities.Umbraco;

namespace Core.Interfaces;

public interface IUmbracoService
{
    Task<IEnumerable<FilteredContent>> GetSiteContent(string culture, string umbracoUrl);
    Task<IEnumerable<FilteredContent>> GetCacheContentAsync(string culture, string umbracoUrl);
    Task<SearchResultsDto> Search(string term, string culture, string umbracoUrl);
    Task<IEnumerable<FilteredContent>> GetContent(string culture, string umbracoUrl);
    Task<string> GetCleanContent(string culture, string umbracoUrl);
}
