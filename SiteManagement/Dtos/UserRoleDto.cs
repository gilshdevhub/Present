using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class UserRoleDto
{
    [Required]
    public IEnumerable<string> Roles { get; set; }

    [Required]
    [MaxLength(60)]
    public string UserName{ get; set; }
}
