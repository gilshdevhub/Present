namespace Core.Entities;

public class SingleSupplier
{
    public Guid Id { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime DecisionDate { get; set; }
    public string SupplierName { get; set; }
    public string ReferentName { get; set; }
    public string ReferentMail { get; set; }
    public string? Subject { get; set; }
    public string? UpdatingUser { get; set; }
    public virtual ICollection<TenderDocuments>? Documentation { get; set; }
    public string Language { get; set; }
    public int Domain { get; set; }
    public int TypeOfTender { get; set; }
    public int Page { get; set; }
    public DateTime UpdateDate { get; set; }
    public virtual MailingList? MailingList { get; set; }
}

public class SingleSupplierDto
{
    public Guid Id { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime DecisionDate { get; set; }
    public string SupplierName { get; set; }
    public string ReferentName { get; set; }
    public string ReferentMail { get; set; }
    public string? Subject { get; set; }
    public string UpdatingUser { get; set; }
    public virtual ICollection<TenderDocumentsDto>? Documentation { get; set; }
    public string Language { get; set; }
    public int Filed { get; set; }
    public int Category { get; set; }
    public int Type { get; set; }
    public DateTime UpdateDate { get; set; }
    public virtual MailingList? MailingList { get; set; }
}
