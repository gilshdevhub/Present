namespace Core.Entities;

public class Tenders
{
    public Guid Id { get; set; }
    public DateTime PublishDate { get; set; }
    public string? ReferentName { get; set; }
    public string? ReferentMail { get; set; }
    public int Domain { get; set; }
    public int TypeOfTender { get; set; }
    public int Page { get; set; }
    public int TenderNumber { get; set; }
    public string TenderName { get; set; }
    public DateTime? ClarifyingDate { get; set; }
    public DateTime? BiddingDate { get; set; }
    public string? WinningSupplier { get; set; }
    public DateTime? WinningDate { get; set; }
       public virtual ICollection<TenderDocuments>? Documentation { get; set; }
    public string? Description { get; set; }
    public string? Biddings { get; set; }
    public string Language { get; set; }
    public DateTime UpdateDate { get; set; }
    public string UpdatingUser { get; set; }
    public string? WaitingSupplier { get; set; }
    public string? WinningAmount { get; set; }
    public virtual MailingList? MailingList { get; set; }
}

public class TendersDto
{
    public DateTime? BiddingDate { get; set; }
    public string? Biddings { get; set; }
    public int? Category { get; set; }
    public DateTime? ClarifyingDate { get; set; }
    public string? Description { get; set; }
    public ICollection<TenderDocumentsDto>? Documentation { get; set; }
    public int? Filed { get; set; }
    public Guid Id { get; set; }
    public string? Language { get; set; }
      public ICollection<MeetingsDto>? Meetings { get; set; }
    public DateTime? PublishDate { get; set; }
    public string? ReferentMail { get; set; }
    public string? ReferentName { get; set; }
    public string? TenderName { get; set; }
    public int? TenderNumber { get; set; }
    public int? Type { get; set; }
    public DateTime UpdateDate { get; set; }
    public string UpdatingUser { get; set; }
    public string? WaitingSupplier { get; set; }
    public string? WinningAmount { get; set; }
    public DateTime? WinningDate { get; set; }
    public string? WinningSupplier { get; set; }
}
