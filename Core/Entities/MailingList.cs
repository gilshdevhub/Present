namespace Core.Entities;

public class MailingList
{
    public int MailingListId { get; set; }
    public string Mails { get; set; }
    public string Name { get; set; }
    public Guid? ExempId { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual ExemptionNotices ExemptionNotices { get; set; }
    public Guid? TenderId { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual Tenders Tenders { get; set; }
    public Guid? SingleId { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual SingleSupplier SingleSupplier { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public Guid? PlanningAndRatesId { get; set; }
    public virtual PlanningAndRates PlanningAndRates { get; set; }
    public int Step { get; set; }
    public int Domain { get; set; }
    public int TypeOfTender { get; set; }
    public int Page { get; set; }
}
