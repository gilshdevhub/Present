using API.Dtos.ClosedStationsAndLines;
using Asp.Versioning;
using Core.Enums;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace API.Controllers;
[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class ClosedStationsAndLinesController : BaseApiController
{
    private readonly IClosedStationsService _closedStationsService;
    private readonly IVersioningService _versioningService;
    public ClosedStationsAndLinesController(IClosedStationsService closedStationsService, IVersioningService versioningService)
    {
        _closedStationsService= closedStationsService;
        _versioningService= versioningService;
    }

    [HttpGet("GetUpdatedList")]
    public async Task<ActionResult<IEnumerable<ClosedStationsAndLineResponse>>> GetClosedStationsAndLinesOutput(int getVersion)
    {
        try
        {
            var version = await _versioningService.GetVersionAsync(Versioning.ClosedStationsAndLines).ConfigureAwait(false);
            ClosedStationsAndLineResponse result;
            if (version.VersionId > getVersion)
            {
                result = new ClosedStationsAndLineResponse()
                {
                    CurrentVersion = version.VersionId,
                    ClosedList = JsonSerializer.Deserialize<IEnumerable<ClosedStationsAndLinesOutDto>>(JsonSerializer.Serialize((await _closedStationsService.GetClosedStationsAndLinesAsync().ConfigureAwait(false)).Where(x => x.ValidTo >= DateTime.Now))).ToList()
                };
                return Ok(result);
            }
            else if (version.VersionId == getVersion)
            {
                return BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] {String.Format( "Version {0} has no updates", getVersion) }));
            }
            return BadRequest(new ApiErrorResponse(statusCode: (int)HttpStatusCode.BadRequest, new string[] { String.Format("Version {0} is incompatible.", getVersion) }));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiErrorResponse { Message = ex.Message, StatusCode = (int)HttpStatusCode.InternalServerError });
        }
    }
                 
      }
