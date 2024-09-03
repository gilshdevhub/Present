using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos;

public class NewRequestForEscortDto
{
    [Required]
    [MaxLength(10)]
    public string CustomerPhone { get; set; } = "0000000000";
    [Required]
    public int Origen { get; set; }
    [Required]
    public int Destination { get; set; }
    public SystemTypes SystemTypeId { get; set; }
    public DateTime? DepartureDateTime { get; set; }
    public DateTime? ArrivalDateTime { get; set; }
    public string? TrainNumber { get; set; }
    [Range(1,9)]
    public int OrigenPlatform { get; set; }
    [Range(1, 9)]
    public int DestinationPlatform { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisableType { get; set; }
}


