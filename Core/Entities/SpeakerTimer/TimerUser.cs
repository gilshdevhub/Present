using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.SpeakerTimer;


public class TimerUser
{
    public int Id {  get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    [Column(TypeName = "varchar(8)")]
    public string Password { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public Discussion? Discussion { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public virtual TimerRole? TimerRole { get; set; }
    public int RoleId { get; set; }

}