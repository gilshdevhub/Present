using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class MailingListPutDto
{
    public int MailingListId { get; set; }
    public string Mails { get; set; }
    public Guid? ExempId { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual ExemptionNotices ExemptionNotices { get; set; }
    public Guid? TenderId { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual Tenders Tenders { get; set; }
    public Guid? SingleId { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual SingleSupplier SingleSupplier { get; set; }
    public Guid? PlanningAndRatesId { get; set; }
    public bool All { get; set; }
    public bool TendersType { get; set; }
    public bool ExemptionNoticesType { get; set; }
    public bool SingleSupplierType { get; set; }
    public bool PlanningAndRatesType { get; set; }
    public int Domain { get; set; }
    public int TypeOfTender { get; set; }
    public int Page { get; set; }
}
