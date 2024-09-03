using AutoMapper;
using Core.Entities.Stations;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

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

    [HttpGet("GetSvgElements")]
    public async Task<ActionResult<IEnumerable<ImageElementsDto>>> GetSvgElements()
    {
        IEnumerable<StationImage> stationImage = await _stationImageService.GetAllSvgElementsAsync().ConfigureAwait(false);
        IEnumerable<ImageElementsDto> imageElements = _mapper.Map<IEnumerable<ImageElementsDto>>(stationImage);
        return Ok(imageElements);
    }

    [HttpGet("BuildStationSVG")]
    public async Task<ActionResult<string>> BuildStationImage()
    {
        string stationImage = (await _stationImageService.BuildSVGAsync().ConfigureAwait(false));
        return Ok(stationImage);
    }

    [HttpGet("StationPathDeactivate")]
    public async Task<ActionResult<bool>> StationActivation(int elementID, bool closed, DateTime fromdate, DateTime? todate, bool betweenDatesInactive)
    {
        StationImage element = (await _stationImageService.GetStationsImageContentAsync().ConfigureAwait(false)).FirstOrDefault(p => p.Id == elementID);

        bool _stationImage = (await _stationImageService.UpdateElementActivationAsync(element, closed, fromdate, betweenDatesInactive, todate).ConfigureAwait(false));

        return Ok(_stationImage);
    }
}
