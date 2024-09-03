using Core.Entities;
using Core.Entities.Umbraco;
using Core.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class SearchService : ISearchService
{
    private readonly ICacheService _cacheService;
    private readonly ITendersService _tendersService;
    private readonly ISingleSupplierIService _singleSupplierService;
    private readonly IExemptionNotices _exemptionNotices;
    private readonly IPlanningAndRatesService _planningAndRatesService;
    private readonly ITendersCommonService _tendersCommonService;
    private readonly HttpClient _httpClientService;
    private readonly IConfiguration _configuration;
    public string umbracoKey = string.Empty;
    public SearchService(ICacheService cacheService, ITendersService tendersService, ISingleSupplierIService singleSupplierService,
        IExemptionNotices exemptionNotices, IPlanningAndRatesService planningAndRatesService, ITendersCommonService tendersCommonService, HttpClient httpClientService, IConfiguration configuration)
    {
        _cacheService = cacheService;
        _tendersService = tendersService;
        _singleSupplierService = singleSupplierService;
        _exemptionNotices = exemptionNotices;
        _planningAndRatesService = planningAndRatesService;
        _tendersCommonService = tendersCommonService;
        _httpClientService = httpClientService;
        _configuration = configuration;
       
    }

    public async Task<SearchDto> Search(string term, string culture)
    {
        SearchDto searchResultsDto = new();

        searchResultsDto.tenderDocumentsDto = JsonConvert.DeserializeObject<IEnumerable<TenderDocumentsDto>>(JsonConvert.SerializeObject(
            (await _tendersCommonService.GetTenderDocumentsContentAsync().ConfigureAwait(false))                                              .Where(x => x.DocName != null && x.DocName.Contains(term)).ToArray()
        ));
        searchResultsDto.tendersDto = (await _tendersService.GetTendersAsync().ConfigureAwait(false))                                      .Where(x => (x.Description != null && x.Description.Contains(term)) ||
                                      (x.TenderName != null && x.TenderName.Contains(term))).ToArray();
        searchResultsDto.singleSupplierDto = (await _singleSupplierService.GetSuppliersAsync().ConfigureAwait(false))                                             .Where(x => (x.SupplierName != null && x.SupplierName.Contains(term)) ||
                                             (x.Subject != null && x.Subject.Contains(term))).ToArray();
        searchResultsDto.exemptionNoticesDto = (await _exemptionNotices.GetExemptionNoticesAsync().ConfigureAwait(false))                                               .Where(x => (x.SupplierName != null && x.SupplierName.Contains(term)) ||
                                               (x.Subject != null && x.Subject.Contains(term))).ToArray();
        searchResultsDto.planningAndRatesDto = (await _planningAndRatesService.GetPlanningAndRatesAsync().ConfigureAwait(false))                                               .Where(x => (x.PlanningAreas != null && x.PlanningAreas.Contains(term)) ||
                                               (x.Subject != null && x.Subject.Contains(term))).ToArray();

        return searchResultsDto;

    }
}
