using API.Dtos.RailUpdates;
using AutoMapper;
using Core.Entities.RailUpdates;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.RailUpdates;

[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class RailUpdatesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IRailUpdatesService _railUpdatesService;
    private readonly IConfigurationService _configurationService;
    private readonly IHttpClientService _httpClient;

    public RailUpdatesController(IMapper mapper, IRailUpdatesService railUpdatesService, IConfigurationService configurationService, IConfiguration configuration, IHttpClientService httpClient)
    {
        _mapper = mapper;
        _railUpdatesService = railUpdatesService;
        _configurationService = configurationService;
        _httpClient = httpClient;
    }

    [HttpGet("Old_GetRailGeneralUpdates")]
       public async Task<ActionResult<IEnumerable<RailUpdateResponseDto>>> GetRailGeneralUpdates([FromQuery] RailGeneralUpdatesRequestDto request)
    {
        RailUpdate railUpdate = await _railUpdatesService.GetRailGeneralUpdatesAsync(request.LanguageId).ConfigureAwait(false);
        IEnumerable<RailUpdateResponseDto> updates =
            _mapper.Map<IEnumerable<RailUpdateResponseDto>>(railUpdate.Data, opts => opts.Items["languageId"] = request.LanguageId);
        return Ok(updates);
    }

    [HttpGet("Old_special")]
    public async Task<ActionResult<IEnumerable<RailUpdateResponseDto>>> GetRailSpecialUpdates([FromQuery] RailSpecialUpdatesRequestDto request)
    {
        RailUpdate railUpdate = await _railUpdatesService.GetRailSpecialUpdatesAsync(request.OriginStationId, request.TargetStationId).ConfigureAwait(false);
        IEnumerable<RailUpdateResponseDto> updates =
            _mapper.Map<IEnumerable<RailUpdateResponseDto>>(railUpdate.Data, opts => opts.Items["languageId"] = request.LanguageId);
        return Ok(updates);
    }

                     
       [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetRailGeneralUpdatesFromUmbraco([FromQuery] RailGeneralUpdatesRequestDto request)
    {
        var isUmbraco = (await _configurationService.GetConfigurationsBySystemTypeAsync(request.SystemType).ConfigureAwait(false))
           .FirstOrDefault(x => x.Key == "GetUmbracoGeneralUpdates");
        if (isUmbraco.Value == "false")
        {
            RailUpdate railUpdate = await _railUpdatesService.GetRailGeneralUpdatesAsync(request.LanguageId).ConfigureAwait(false);
            IEnumerable<RailUpdateResponseDto> updates =
                _mapper.Map<IEnumerable<RailUpdateResponseDto>>(railUpdate.Data, opts => opts.Items["languageId"] = request.LanguageId);
            return Ok(updates);
        }
        else
        {
            IEnumerable<RailUpdateResponseUmbracoDto> railUpdates = await _railUpdatesService.GetRailUpdatesNewAsync(request.LanguageId).ConfigureAwait(false);
                       return Ok(railUpdates.Where(x => (x.UpdateType == "Permanent"|| x.UpdateType == "Regular")));
        }
    }

    [HttpGet("GetRailGeneralUpdatesFromUmbracoByStationId")]
    public async Task<ActionResult<IEnumerable<RailUpdateResponseUmbracoDto>>> GetRailGeneralUpdatesFromUmbracoByStationId([FromQuery] RailUpdateRequestbyStationIdDto request)
    {
               IEnumerable<RailUpdateResponseUmbracoDto> railUpdate = await _railUpdatesService.UmbracoGeneralUpdatesByLanguageAsync(request.LanguageId, request.StationId).ConfigureAwait(false);

        return Ok(railUpdate);
    }

    [HttpPost("SetCache")]
    public async Task<ActionResult<Boolean>> SetCache()
    {
               try
        {
            await _railUpdatesService.SetRailUpdatesNewAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(true);
    }


}
