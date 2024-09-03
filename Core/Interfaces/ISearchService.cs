using Core.Entities.Umbraco;

namespace Core.Interfaces;

public interface ISearchService
{
    Task<SearchDto> Search(string term, string culture);
}
