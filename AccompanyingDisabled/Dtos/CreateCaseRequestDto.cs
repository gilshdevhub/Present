namespace AccompanyingDisabled.Dtos;

public class CreateCaseRequestDto
{
    public bool issendinternalemail { get; set; }
    public string? new_ravkavnumber { get; set; }
    public string? new_idnum { get; set; }
    public string? fullname { get; set; }
    public string? firstname { get; set; }
    public string? lastname { get; set; }
    public string? token { get; set; }
    public string? emailaddress1 { get; set; }
    public string? address1_telephone1 { get; set; }
    public string? new_customerid { get; set; }
    public string? description { get; set; }
    public string? new_casechannel { get; set; }
    public string? new_casesubject1 { get; set; }
    public string? new_casesubject2 { get; set; }
    public string? new_casesubject3 { get; set; }
    public string? address1_line1 { get; set; }
    public string? address1_postalcode { get; set; }
    public string? address1_postofficebox { get; set; }
    public string? address1_city { get; set; }
    public int new_capability { get; set; }
    public string? new_escort { get; set; }
    public string? new_travelfromdate { get; set; }
    public string? new_travelbackdate { get; set; }
    public string? new_occasiondate { get; set; }
    public string? new_arrival_time_one_way { get; set; }
    public string? new_arrival_time_back { get; set; }
    public string? new_initialstation { get; set; }
    public string? new_destination { get; set; }
    public string? new_departure_satation_back { get; set; }
    public string? new_arrivale_station_back { get; set; }
    public string? new_desc1 { get; set; }
    public string? new_desc2 { get; set; }
    public string? new_desc3 { get; set; }
    public string? new_desc4 { get; set; }
    public string? new_desc5 { get; set; }
    public string? new_demandnumber { get; set; }
    public string? ownerid { get; set; }
    public FileRequestDto[] files { get; set; }
    public string? new_workid { get; set; }
    public string? new_intialstation { get; set; }
    public string? new_site { get; set; }
    public string? new_casemahut1 { get; set; }
    public string? new_casemahut2 { get; set; }
    public string? new_facilitysite { get; set; }
    public string? new_crosspoint { get; set; }
    public string? new_incidentreporter { get; set; }
    public int new_reporterjob { get; set; }
    public string? new_staionname { get; set; }
    public string? employeeid { get; set; }
    public string? new_trainnumber { get; set; }
}