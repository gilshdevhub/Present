using Core.Entities;

namespace SiteManagement.Dtos;

public class TendersPostDto
{
    public DateTime PublishDate { get; set; }
    public string ReferentName { get; set; }
    public string ReferentMail { get; set; }
    public int Filed { get; set; }
    public int Category { get; set; }
    public int Type { get; set; }
    public int TenderNumber { get; set; }
    public string TenderName { get; set; }
    public DateTime ClarifyingDate { get; set; }
    public DateTime BiddingDate { get; set; }
    public string? WinningSupplier { get; set; }
    public DateTime WinningDate { get; set; }
    public ICollection<Meetings>? Meetings { get; set; }
    public ICollection<TenderDocuments>? Documentation { get; set; }
    public string? Description { get; set; }
    public string? Biddings { get; set; }
    public string Language { get; set; }
    public DateTime UpdateDate { get; set; }
    public string? UpdatingUser { get; set; }
    public string? WaitingSupplier { get; set; }
    public int MailingListId { get; set; }
    public string? WinningAmount { get; set; }
}

public class TendersPutDto
{
    public Guid TenderId { get; set; }
    public DateTime PublishDate { get; set; }
    public string? ReferentName { get; set; }
    public string? ReferentMail { get; set; }
    public int? Filed { get; set; }
    public int? Category { get; set; }
    public int? Type { get; set; }
    public int? TenderNumber { get; set; }
    public string? TenderName { get; set; }
    public DateTime? ClarifyingDate { get; set; }
    public DateTime? BiddingDate { get; set; }
    public string? WinningSupplier { get; set; }
    public DateTime? WinningDate { get; set; }
       public ICollection<Meetings>? Meetings { get; set; }
    public ICollection<TenderDocuments>? Documentation { get; set; }
    public string? Description { get; set; }
    public string? Biddings { get; set; }
    public string? Language { get; set; }
    public DateTime UpdateDate { get; set; }
    public string? UpdatingUser { get; set; }
    public string? WaitingSupplier { get; set; }
    public int? MailingListId { get; set; }
    public string? WinningAmount { get; set; }
}
