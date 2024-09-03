using System.ComponentModel.DataAnnotations;

namespace SpeakersTimer.Dtos
{
    public class LoginTimerDto
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
