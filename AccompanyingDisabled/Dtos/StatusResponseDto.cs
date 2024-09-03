using System.ComponentModel.DataAnnotations;

namespace AccompanyingDisabled.Dtos
{
    public class StatusResponseDto
    {
        [Required]
        public int statusId { get; set; }
        [Required]
        public string statusDesc { get; set; }
        [Required]
        [Range(1, 3)]
        public int statusFor { get; set; }
    }
}
