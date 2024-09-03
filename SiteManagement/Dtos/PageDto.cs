using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class PageDto
{
    public int Id { get; set; }
    [MaxLength(120)]
    public string Folder { get; set; }
    [MaxLength(120)]
    public string Title { get; set; }
    [MaxLength(120)]
    public string FrontPath { get; set; }
    [MaxLength(120)]
    public string Component { get; set; }
    [MaxLength(120)]
    public string ClassProp { get; set; }
    [MaxLength(120)]
    public string EditComponent { get; set; }
    [MaxLength(120)]
    public string Controller { get; set; }
    [MaxLength(120)]
    public IEnumerable<string> Roles { get; set; }
}


public class RolesDto
{
    [MaxLength(120)]
    public string Name { get; set; }
}
