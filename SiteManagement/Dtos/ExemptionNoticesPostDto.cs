using Core.Entities;

namespace SiteManagement.Dtos;

public class ExemptionNoticesPostDto
{
    public DateTime PublishDate { get; set; }
    public string SupplierName { get; set; }
    public string ReferentName { get; set; }
    public string ReferentMail { get; set; }
    public string Subject { get; set; }
    public string UpdatingUser { get; set; }
    public ICollection<TenderDocuments>? Documentation { get; set; }
    public string Language { get; set; }
    public int Filed { get; set; }
    public int Category { get; set; }
    public int Type { get; set; }
    public int? MailingListId { get; set; }
    public DateTime UpdateDate { get; set; }
    public MailingList? MailingList { get; set; }
}
public class ExemptionNoticesPutDto
{
    public Guid ExempId { get; set; }
    public DateTime PublishDate { get; set; }
    public string SupplierName { get; set; }
    public string ReferentName { get; set; }
    public string ReferentMail { get; set; }
    public string Subject { get; set; }
    public string UpdatingUser { get; set; }
    public ICollection<TenderDocuments> Documentation { get; set; }
    public string Language { get; set; }
    public int Filed { get; set; }
    public int Category { get; set; }
    public int Type { get; set; }
    public int MailingListId { get; set; }
    public DateTime UpdateDate { get; set; }
    public MailingList MailingList { get; set; }
}
