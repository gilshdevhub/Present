using Core.Entities.MyTravel;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Core.Entities.Vouchers.MyTrip;

namespace API.Controllers;

public class MyTripController : BaseApiController
{
    private readonly IMyTripService _mytripService;



    public MyTripController(IMyTripService mytripService)
    {
        _mytripService = mytripService;
    }

    [HttpPost("GetClosestSations")]
    public async Task<ActionResult<IEnumerable<VisaTrainMainData>>> GetClosestSations([FromBody] ClosedTrainsRequestDto request)
    {
        try
        {
            IEnumerable<VisaTrainMainData> distance = await _mytripService.GetClosestSations(request).ConfigureAwait(false);
            return Ok(distance);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}