using Asp.Versioning;
using AutoMapper;
using Core.Entities.Fares;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiVersion("0.9", Deprecated = true)]
public class FaresController : BaseApiController
{
    private readonly IFaresService _faresService;
    private readonly IMapper _mapper;

    public FaresController(IFaresService faresService, IMapper mapper)
    {
        _faresService = faresService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<Object>> GetFares([FromQuery] FaresRequestData requestData)
    {
        var result = await _faresService.GetFares(requestData).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("GetCodesData")]
    public async Task<ActionResult<CodesDataDto>> GetCodesData()
    {
        CodesDataDto result = await _faresService.GetCodesData().ConfigureAwait(false);
        return Ok(result);
    }

}
