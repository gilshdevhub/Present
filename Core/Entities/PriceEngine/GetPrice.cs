using Core.Enums;
using System.ComponentModel.DataAnnotations;
namespace Core.Entities.PriceEngine;
public class GetAllPriceResultObject
{
    public List<SourceToDestinationDTO>? AllSourceToDestination { get; set; }
    public List<Profile_Discount_ContractDTO>? AllProfileDiscount { get; set; }
}
public class GetAllPriceResultObjectWithNotes
{
    public List<SourceToDestinationDTO>? AllSourceToDestination { get; set; }
    public List<Profile_Discount_ContractDTO>? AllProfileDiscount { get; set; }
    public List<PriceNotes>? PriceNotes { get; set; }
}


public class SourceToDestinationDTO
{

    public int? From_Station_Code_ISR { get; set; }

    public int? To_Station_Code_ISR { get; set; }
    public int? Distance_Code { get; set; }
       public float? S_Price { get; set; }
       public float? D_Price { get; set; }
       public float? M_Price { get; set; }
}


public class Profile_Discount_ContractDTO
{
    public int? TicketType { get; set; }
          public int? Profile_ID { get; set; }
    public float? Discount_Rate { get; set; }


}






public class getPriceRequest
{
    public SystemTypes? SystemType { get; set; }

    public int? RequestId { get; set; }

    [Required(ErrorMessage = "From_Station_Code_ISR is a Required field")]
    public int? From_Station_Code_ISR { get; set; }

    [Required(ErrorMessage = "To_Station_Code_ISR is a Required field")]
    public int? To_Station_Code_ISR { get; set; }
    public int? Profile_ID { get; set; } = null;
}

public class priceDetails

{


    public string? Heb_Contract_Description { get; set; }

    public string? Eng_Contract_Description { get; set; }

    public string? Arb_Contract_Description { get; set; }

    public string? Rus_Contract_Description { get; set; }

    public int? TicketType { get; set; }
    public int? DistanceCode { get; set; }
    public int? Profile_ID { get; set; }

    public int? Full_Price { get; set; }

    public double? Final_Price { get; set; }

    public string? Heb_Note_To_The_Passenger { get; set; }

    public string? Eng_Note_To_The_Passenger { get; set; }

    public string? Arb_Note_To_The_Passenger { get; set; }

    public string? Rus_Note_To_The_Passenger { get; set; }

}


