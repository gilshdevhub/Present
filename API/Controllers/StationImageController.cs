using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class StationImageController : BaseApiController
{
    private readonly IStationImage _stationImageService;
    private readonly IMapper _mapper;
    public StationImageController(IStationImage stationImageService, IMapper mapper)
    {
        _mapper = mapper;
        _stationImageService = stationImageService;
    }

    [HttpGet("GetStationSVG")]
    public async Task<ActionResult<string>> GetStationImage()
    {
        var stationImage = await _stationImageService.GetStationSVGAsync().ConfigureAwait(false);
        return Ok(stationImage);
    }

    [HttpGet("GetStationSVGPerLanguage")]
    public async Task<ActionResult<object>> GetStationSVGPerLanguage()
    {
        var stationImage = await _stationImageService.GetStationSVGPerLanguageAsync().ConfigureAwait(false);
        return Ok(stationImage);
    }

}
