using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PriceEngine.Dtos;


public class getAllPriceRequestDto
{
    [Required(ErrorMessage = "SystemType is a Required field")]
    public SystemTypes? SystemType { get; set; }

}



public class getPriceRequestDto
{
    [Required(ErrorMessage = "SystemType is a Required field")]
    public SystemTypes? SystemType { get; set; }
    
    public int? RequestId { get; set; }

    [Required(ErrorMessage = "From_Station_Code_ISR is a Required field")]
    public int? From_Station_Code_ISR { get; set; }
   
    [Required(ErrorMessage = "To_Station_Code_ISR is a Required field")]
    public int? To_Station_Code_ISR { get; set; }
    public int? Profile_ID { get; set; } = null;
}



public class getProfilesDto
{
   
    [Required(ErrorMessage = "SystemType is a Required field")]
    public SystemTypes? SystemType { get; set; }
    [Required(ErrorMessage = "RequestId is a Required field")]
    public int? RequestId { get; set; }
    //public int LangId { get; set; } = 0;
}
