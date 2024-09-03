using Microsoft.AspNetCore.Http;

namespace Core.Entities;

public class TenderDocuments
{
    public int Id { get; set; }
    public string? DocName { get; set; }
    public int DocType { get; set; }
    public string? DocDisplay { get; set; }
    public Guid? TendersId { get; set; }
    public virtual Tenders? Tenders { get; set; }
    public Guid? SingleSupplierId { get; set; }
    public virtual SingleSupplier? SingleSupplier { get; set; }
    public Guid? ExemptionNoticesId { get; set; }
    public virtual ExemptionNotices? ExemptionNotices { get; set; }
    public Guid? PlanningAndRatesId { get; set; }
    public virtual PlanningAndRates? PlanningAndRates { get; set; }
}
public class UploadFile
{
    public IFormFile FileToLoad { get; set; }
    public int DocType { get; set; }
    public string? DocDisplay { get; set; }

}

public class TenderDocumentsDto
{
    public int Id { get; set; }
    public string? DocName { get; set; }
    public int DocType { get; set; }
    public string? DocDisplay { get; set; }
}
