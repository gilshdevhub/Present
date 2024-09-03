using Core.Entities;

namespace SiteManagement.Dtos;

public class PlanningAndRatesPostDto
{
    public Guid Id { get; set; }
    public string? SerialNumber { get; set; }
    public string PlanningAreas { get; set; }
    public string? Subject { get; set; }
    public string UpdatingUser { get; set; }
    public virtual ICollection<TenderDocumentsDto>? Documentation { get; set; }
    public string Language { get; set; }
    public int Filed { get; set; }
    public int Category { get; set; }
    public int Type { get; set; }
    public DateTime UpdateDate { get; set; }
}
public class PlanningAndRatesPutDto
{
    public Guid Id { get; set; }
    public string? SerialNumber { get; set; }
    public string PlanningAreas { get; set; }
    public string? Subject { get; set; }
    public string UpdatingUser { get; set; }
    public virtual ICollection<TenderDocuments> Documentation { get; set; }
    public string Language { get; set; }
    public int Filed { get; set; }
    public int Category { get; set; }
    public int Type { get; set; }
    public DateTime UpdateDate { get; set; }
}
