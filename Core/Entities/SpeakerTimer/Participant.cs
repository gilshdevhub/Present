using Core.Entities.Vouchers;
using Core.Enums;
using Core.Helpers.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.SpeakerTimer;


public class Participant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public int Duration { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string JobRole { get; set; }
    [Newtonsoft.Json.JsonIgnore]
    public Discussion? Discussion { get; set; }
}