using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Forms;

public class FullForms
{
    public string? address1_telephone1 { get; set; } = "";
    public string? lastname { get; set; } = "";
    public string? firstname { get; set; } = "";
    public DateTime new_occasiondate { get; set; } 
    public string? new_destination { get; set; }   = "";
    public string? new_initialstation { get; set; }= "";
    public IFormFile[]? files { get; set; } 
    public string? new_casesubject1 { get; set; }= "";
    public string? new_casesubject2 { get; set; }= "";
    public string? new_casesubject3 { get; set; }= "";
    public string? new_customerid { get; set; }= "";
    public string? address1_city { get; set; } = "";
    public string? address1_line1 { get; set; } = "";
    public string? address1_postalcode { get; set; } = "";
    public string? address1_postofficebox { get; set; } = "";
    public string? emailaddress1 { get; set; } = "";
    public string? description { get; set; }                = "";
    public string? new_ravkavnumber { get; set; }           = "";
    public int formId { get; set; }                       
    public string? new_name { get; set; }                   = "";
    public string? new_idnum { get; set; }                  = "";
    public string? token { get; set; }                      = "";
    public string? new_casechannel { get; set; }            = "";
    public string? new_casemahut1 { get; set; }             = "";
    public string? new_casemahut2 { get; set; }             = "";
    public string? new_travelfromdate { get; set; }         = "";
    public string? new_travelbackdate { get; set; }         = "";
    public string? new_departure_satation_back { get; set; }= "";
    public string? new_arrival_time_back { get; set; } = "";
    public string? new_arrival_time_one_way { get; set; } = "";
    public string? new_arrivale_station_back { get; set; }= "";
    public string? new_capability { get; set; } = "";
    public string? new_escort { get; set; }          = "";
    public string? new_desc1 { get; set; }           = "";
    public string? new_desc2 { get; set; }           = "";
    public string? new_desc3 { get; set; }           = "";
    public string? new_desc4 { get; set; }           = "";
    public string? new_desc5 { get; set; }           = "";
    public string? ownerid { get; set; }             = "";
    public string? new_workid { get; set; }          = "";
    public string? new_crosspoint { get; set; }      = "";
    public string? new_mahut1 { get; set; }          = "";
    public string? new_site { get; set; }            = "";
    public string? new_facilitysite { get; set; }    = "";
    public string? new_demandnumber { get; set; }    = "";
    public string? new_incidentreporter { get; set; }= "";
    public string? new_reporterjob { get; set; } = "";
    public string? new_staionname { get; set; }     = "";
    public string? issendinternalemail { get; set; }= "";
}
public class stationFromClient
{
    public int Id { get; set; }
    public string Name { get; set; }
}
public class FormsIdThrees
{
    [ Key]
    public int formId { get; set; }
    public string name { get; set; }
    public string? firstThree { get; set; }
    public string? secondThree { get; set; }
    public string? thiredThree { get; set; }
}

public class FormsResponse
{
    public string guid { get; set; }
    public string code { get; set; }
}
