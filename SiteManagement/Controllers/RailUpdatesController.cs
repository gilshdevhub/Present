using AutoMapper;
using Core.Entities.RailUpdates;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteManagement.Controllers.RailUpdates;

public class RailUpdatesController : BaseApiController
{
    private readonly IRailUpdatesService _railUpdatesService;

    public RailUpdatesController(IRailUpdatesService railUpdatesService)
    {
        _railUpdatesService = railUpdatesService;
    }

    [Authorize(Policy = "PageRole")]
    [HttpPost("SetCache")]
    public async Task<ActionResult<bool>> SetCache()
    {
        bool res;
        try
        {
              
           res =  await _railUpdatesService.SetRailUpdatesWithTimerAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(res);
    }
}
