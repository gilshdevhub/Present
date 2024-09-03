namespace Core.Entities;

public class PlanningAndRates
{
    public Guid Id { get; set; }
    public string? SerialNumber { get; set; }
    public string? PlanningAreas { get; set; }
    public string? Subject { get; set; }
    public string? UpdatingUser { get; set; }
    public virtual ICollection<TenderDocuments>? Documentation { get; set; }
    public string? Language { get; set; }
    public int Domain { get; set; }
    public int TypeOfTender { get; set; }
    public int Page { get; set; }
    public DateTime UpdateDate { get; set; }
    public virtual MailingList? MailingList { get; set; }

}

public class PlanningAndRatesDto
{
    public Guid Id { get; set; }
    public string? SerialNumber { get; set; }
    public string? PlanningAreas { get; set; }
    public string? Subject { get; set; }
    public string? UpdatingUser { get; set; }
    public virtual ICollection<TenderDocumentsDto>? Documentation { get; set; }
    public string? Language { get; set; }
    public int Filed { get; set; }
    public int Category { get; set; }
    public int Type { get; set; }
    public DateTime UpdateDate { get; set; }
}
