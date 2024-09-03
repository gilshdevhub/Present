using AutoMapper;
using Core.Entities.Configuration;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class ConfigurationController : BaseApiController
{
    private readonly IConfigurationService _configurationService;
    private readonly IMapper _mapper;
    public ConfigurationController(IConfigurationService configurationService, IMapper mapper)
    {
        _configurationService = configurationService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<IEnumerable<ConfigurationResponseDto>> GetAllConfigurations()
    {
        IEnumerable<ConfigurationParameter> configurations = await _configurationService.GetAllItemsNoCache();
        IEnumerable<ConfigurationResponseDto> result = _mapper.Map<IEnumerable<ConfigurationParameter>, IEnumerable<ConfigurationResponseDto>>(configurations);
        return result;
    }

    [HttpGet("systemtypes")]
    [Authorize]
    public Task<IEnumerable<SystemType>> GetSystemTypes()
    {
        return _configurationService.GetSystemTypesAsync();
    }


    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> AddConfiguration(ConfigurationRequestDto config)
    {
        ConfigurationParameter configToSave = _mapper.Map<ConfigurationParameter>(config);
        ConfigurationParameter newConfiguration = await _configurationService.Add(configToSave);
        var error = new ClientErrorData();
        if (newConfiguration == null)
        {
            return BadRequest(new ApiErrorResponse { Message = "פרמטר עם שם כזה לסביבה זו כבר קיים במערכת" , StatusCode = 400 });
        }
        else
        {
            IEnumerable<ConfigurationParameter> configurations = await _configurationService.GetAllItemsNoCache();
            return Ok(true);
        }
       
    }

    [Authorize(Policy = "PageRole")]
    [HttpPut]
    public async Task<ActionResult<bool>> UpdateConfiguration(ConfigurationRequestDto config)
    {
        ConfigurationParameter oldObj = await _configurationService.GetItemAsync(config.Key);
        if (oldObj == null)
        {
            return BadRequest(new ApiErrorResponse { Message = "עדכון נכשל", StatusCode = 400 });
        }
        else
        {
            oldObj.ValueWeb = config.ValueWeb;
            oldObj.ValueMob = config.ValueMob;
            oldObj.Description = config.Description;

            bool res = await _configurationService.Update(oldObj);

            if (res)
            {
                IEnumerable<ConfigurationParameter> configurations = await _configurationService.GetAllItemsNoCache();
                return Ok(true);
            }
            else
            {
                return BadRequest(new ApiErrorResponse { Message = "עדכון נכשל", StatusCode = 400 });
            }
        }

      
    }
    [Authorize(Policy = "PageRole")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteConfiguration(string id)
    {
        try
        {
            bool res = await _configurationService.DeleteConfigurationAsync(id);

            if (res)
            {
                IEnumerable<ConfigurationParameter> configurations = await _configurationService.GetAllItemsNoCache();
                return Ok(true);
            }
            else
            {
                return BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
            }
        }
        catch
        {
             return BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }
    }
}
