using Core.Entities.Vouchers;
using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.SpeakerTimer;


public class Discussion
{
    [Newtonsoft.Json.JsonIgnore]
    public ICollection<Participant>? Participant { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public DateTime Date { get; set; }
    [ForeignKey("TimeUser")]
    public TimerUser Owner { get; set; }
}