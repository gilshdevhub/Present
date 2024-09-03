using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class LoginDto
{
    [Required]
    [MaxLength(60)]
    public string UserName { get; set; }
    [Required]
    [MaxLength(60)]
    public string Password { get; set; }
}
