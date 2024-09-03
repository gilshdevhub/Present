using Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteManagement.Dtos;

public class ConfigurationRequestDto
{
    [Required]
    [MaxLength(60)]
    public string Key { get; set; }
    [MaxLength(2048)]
    public string? ValueMob { get; set; }
    [MaxLength(2048)]
    public string? ValueWeb { get; set; }
    [MaxLength(256)]
    public string? Description { get; set; }
}


public class ConfigurationResponseDto
{
    public string Key { get; set; }
    public string? ValueWeb { get; set; }
    public string? ValueMob { get; set; }
    public string? Description { get; set; }
}

public class ConfigurationCriteriaDto : BaseConfigurationCriteriaDto
{
    [Required]
    public string Key { get; set; }
}

public class BaseConfigurationCriteriaDto
{
    [Required]
    public SystemTypes SystemTypeId { get; set; }
}
