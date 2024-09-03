using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class RegisterDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(120)]
    public string DisplayName { get; set; }
    [Required]
    [EmailAddress]
    [MaxLength(120)]
    public string UserName { get; set; }
    [Required]
    [MinLength(8)]
    [MaxLength(60)]
    public string Password { get; set; }
    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; }
    [Required]
    public string[] RoleNames { get; set; }
}
