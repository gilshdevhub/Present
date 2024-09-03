using API.Dtos.Configurations;
using AutoMapper;
using Core.Entities.Configuration;
using Core.Enums;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class ConfigurationController : BaseApiController
{
    private readonly IConfigurationService _configurationService;
    private readonly IMapper _mapper;

    public ConfigurationController(IConfigurationService configurationService, IMapper mapper)
    {
        _configurationService = configurationService;
        _mapper = mapper;
    }

    [HttpGet("bysystemtype")]
    public async Task<ActionResult<IEnumerable<ConfigurationResponseDto>>> GetConfigurationsBySystemType([FromQuery] SystemTypes systemTypeId)
    {
        IEnumerable<Configuration> configurations = await _configurationService.GetConfigurationsBySystemTypeAsync(systemTypeId).ConfigureAwait(false);
        IEnumerable<ConfigurationResponseDto> result = _mapper.Map<IEnumerable<Configuration>, IEnumerable<ConfigurationResponseDto>>(configurations);
        return Ok(result);
    }

    [HttpGet("PushUpdateTime")]
    public async Task<ActionResult<object>> PushUpdateTime(DateTime date)
    {
        var pushWeekHours = await _configurationService.GetWeekDaysAsync(date).ConfigureAwait(false);
        return Ok(JsonConvert.SerializeObject(pushWeekHours));
    }
}
