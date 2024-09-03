using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class MailingListPutDto
{
    [MaxLength(512)]
    public string Mails { get; set; }
    public string Name { get; set; }
    public bool All { get; set; }
    public Guid? ExempId { get; set; }
    public Guid? TenderId { get; set; }
    public Guid? SingleId { get; set; }
    public Guid? PlanningAndRatesId { get; set; }
    public bool TendersType { get; set; }
    public bool ExemptionNoticesType { get; set; }
    public bool SingleSupplierType { get; set; }
    public bool PlanningAndRatesType { get; set; }
    public int Domain { get; set; }
    public int TypeOfTender { get; set; }
    public int Page { get; set; }
    public int Step { get; set; }
}