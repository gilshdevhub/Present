using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

public class EligibilityController : BaseApiController
{
    private readonly IEligibilityService _eligibilityService;

    public EligibilityController(IEligibilityService eligibilityService)
    {
        _eligibilityService = eligibilityService;
    }
  
    [HttpGet("{ravKavNumber}")]
    public async Task<ActionResult<RavKav>> Checking(string ravKavNumber)
    {
        RavKav result = await _eligibilityService.Checking(ravKavNumber).ConfigureAwait(false);
        return Ok(result);
    }
}
