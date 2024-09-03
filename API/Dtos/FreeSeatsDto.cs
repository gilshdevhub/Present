using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos;

public class FreeSeatsRequestDto
{
    [Required]
    public DateTime Date { get; set; }
    [Required]
    [Range(minimum: 1, maximum: 9999)]
    public int OriginStation { get; set; }
    [Required]
    [Range(minimum: 1, maximum: 9999)]
    public int DestinationStation { get; set; }
    [MaxLength(1024)]
    public string TrainNumbers { get; set; }
    [Required]
    [EnumRangeValidator]
    public SystemTypes SystemType { get; set; }
}

public class FreeSeatsResponseDto
{
    public int TrainNumber { get; set; }
    public int FreeSeats { get; set; }
    public DateTime TrainDate { get; set; }
}
