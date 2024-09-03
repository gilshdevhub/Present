using Core.Entities.PagesManagement;
using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class RoleDto
{
    [Required]
    [MaxLength(120)]
    public string Name { get; set; }
    public int Id { get; set; }
    public ICollection<PageRoleNewResponse>? PageRoleNew { get; set; }
}