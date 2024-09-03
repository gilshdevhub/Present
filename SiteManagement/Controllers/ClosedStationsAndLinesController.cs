using Core.Entities.Stations;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;
using System.Net;
using System.Text.Json;

namespace SiteManagement.Controllers;

public class ClosedStationsAndLinesController : BaseApiController
{
    readonly IClosedStationsService _closedStationsService;
    public ClosedStationsAndLinesController(IClosedStationsService closedStationsService)
    {
        _closedStationsService = closedStationsService;
    }

    [HttpGet("GetClosedStationsAndLines")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<ClosedStationsAndLinesDto>>> GetClosedStationsAndLines()
    {
        try
        {
            IEnumerable<ClosedStationsAndLinesDto> result = JsonSerializer.Deserialize<IEnumerable<ClosedStationsAndLinesDto>>(JsonSerializer.Serialize(await _closedStationsService.GetClosedStationsAndLinesAsync()));
            return result.Count() != 0 ? Ok(result) :
            BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] { JsonSerializer.Serialize(result) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message, StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }

    [HttpGet("GetClosedStationsAndLinesById/{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<ClosedStationsAndLinesDto>> GetClosedStationsAndLinesById(int Id)
    {
        try
        {
            ClosedStationsAndLinesDto result = result = JsonSerializer.Deserialize<ClosedStationsAndLinesDto>(JsonSerializer.Serialize(await _closedStationsService.GetClosedStationsAndLinesByIdAsync(Id)));
            return result != null ? Ok(result) :
                BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] { JsonSerializer.Serialize(result) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message, StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }

    [HttpDelete("DeleteClosedStation/{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> DeleteClosedStation(int Id)
    {
        try
        {
            var result = await _closedStationsService.DeleteClosedStationsAndLinesAsync(Id);
            return result != null ? Ok(result) :
                    BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] { JsonSerializer.Serialize(result) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message, StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }
    [HttpPut("UpdateClosedStationsAndLines")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdateClosedStationsAndLines(ClosedStationsAndLinesDto closedUpdate)
    {
        try
        {
            var ent = JsonSerializer.Deserialize<ClosedStationsAndLines>(JsonSerializer.Serialize(closedUpdate));
            var result = await _closedStationsService.UpdateClosedStationsAndLinesAsync(ent);
            return result != null ? Ok(result) :
                      BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] { JsonSerializer.Serialize(result) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message, StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }

    [HttpPost("AddClosedStationsAndLines")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<ClosedStationsAndLinesDto>> AddClosedStationsAndLines(ClosedStationsAndLinesDto closedAdd)
    {
        try
        {
            var ent = JsonSerializer.Deserialize<ClosedStationsAndLines>(JsonSerializer.Serialize(closedAdd));
            var result = await _closedStationsService.AddClosedStationsAndLinesAsync(ent);
            return result != null ? Ok(JsonSerializer.Deserialize<ClosedStationsAndLinesDto>(JsonSerializer.Serialize(result))) :
                    BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] { JsonSerializer.Serialize(result) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message, StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }

}
