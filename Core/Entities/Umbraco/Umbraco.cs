namespace Core.Entities.Umbraco;

public class FilteredContent
{
    public FilteredContent(Guid id, string name, IDictionary<string, object> properties, string url)
    {
        Id = id;
        Name = name;
        Properties = properties;
        Url = url;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IDictionary<string, object> Properties { get; set; }
    public string Url { get; set; }
}
public class SearchResultsDto
{
    public object content { get; set; }
    public IEnumerable<TenderDocumentsDto> tenderDocumentsDto { get; set; }
    public IEnumerable<TendersDto> tendersDto { get; set; }
    public IEnumerable<SingleSupplierDto> singleSupplierDto { get; set; }
    public IEnumerable<ExemptionNoticesDto> exemptionNoticesDto { get; set; }
    public IEnumerable<PlanningAndRatesDto> planningAndRatesDto { get; set; }
}

public class SearchDto
{
    public IEnumerable<TenderDocumentsDto> tenderDocumentsDto { get; set; }
    public IEnumerable<TendersDto> tendersDto { get; set; }
    public IEnumerable<SingleSupplierDto> singleSupplierDto { get; set; }
    public IEnumerable<ExemptionNoticesDto> exemptionNoticesDto { get; set; }
    public IEnumerable<PlanningAndRatesDto> planningAndRatesDto { get; set; }
}
