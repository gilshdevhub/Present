using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Configuration;

public class ConfigurationParameter
{
    [Key]
    public string Key { get; set; }
    public string? ValueMob { get; set; }
    public string? ValueWeb { get; set; }
    public string? Description { get; set; }
}
