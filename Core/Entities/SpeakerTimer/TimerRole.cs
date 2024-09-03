using Core.Entities.Vouchers;
using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.SpeakerTimer;


public class TimerRole
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual TimerUser? TimerUsers { get; set; }
}