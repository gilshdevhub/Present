using Core.Entities.PagesManagement;
using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class UserDto
{
    [MaxLength(120)]
    public string? DisplayName { get; set; }
    public string? Token { get; set; }
    [MaxLength(15)]
    public string? PhoneNumber { get; set; }
    [MaxLength(60)]
    public string UserName { get; set; }
    public IEnumerable<string>? Roles { get; set; }
    public IEnumerable<PageResponse>? Pages { get; set; }
}