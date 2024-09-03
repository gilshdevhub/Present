using API.Dtos;
using AutoMapper;
using Core.Entities.FreeSeats;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Asp.Versioning;

namespace API.Controllers;
[ApiVersion("0.9", Deprecated = true)]
[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class SeatsController : BaseApiController
{
    private readonly IFreeSeatsService _freeSeatsService;
    private readonly IMapper _mapper;

    public SeatsController(IFreeSeatsService freeSeatsService, IMapper mapper)
    {
        _freeSeatsService = freeSeatsService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FreeSeatsResponseDto>>> GetAvailableSeats([FromQuery] FreeSeatsRequestDto request)
    {
        FreeSeatsRequest freeSeatsRequest = _mapper.Map<FreeSeatsRequest>(request);

        var freeSeats = await _freeSeatsService.GetFreeSeatsAsync(freeSeatsRequest).ConfigureAwait(false);

        if (freeSeats?.clsResult?.ReturnCode == 1)
        {
            IEnumerable<FreeSeatsResponseDto> freeSeatsResponseDto = _mapper.Map<IEnumerable<FreeSeatsResponseDto>>(freeSeats.TrainAvailableSeats);
            return Ok(freeSeatsResponseDto);
        }

        return StatusCode((int)HttpStatusCode.InternalServerError, new ApiErrorResponse { Message = "שרות מקומות פנויים נכשל" });
    }
}
