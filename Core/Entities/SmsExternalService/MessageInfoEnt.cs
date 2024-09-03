using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.SmsExternalService;

public class MessageInfoEnt
{
    [Required]
    [MaxLength(1024)]
    public string message { get; set; }
    [Required]
    public string[] numbers { get; set; }
    [Required]
    [EnumRangeValidator]
    public SystemTypes systemType { get; set; }
    [Required]
    [EnumRangeValidator]
    public SmsSubscriberType subscriber { get; set; }
    [Required]
    public int msgQ { get; set; }
}
