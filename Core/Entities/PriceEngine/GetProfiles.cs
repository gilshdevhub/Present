using Core.Enums;
using System.ComponentModel.DataAnnotations;
namespace Core.Entities.PriceEngine;
public class getProfiles
{
       public SystemTypes? SystemType { get; set; }
    public int? RequestId { get; set; }
   
}

public class profileDetails

{
    public int? Profile_Id { get; set; }
    public string? Heb_Profile_Desc { get; set; }
    public string? Eng_Desc { get; set; }
    public string? Arb_Desc { get; set; }
    public string? Rus_Desc { get; set; }
}


