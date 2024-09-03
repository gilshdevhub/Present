using Core.Entities.PriceEngine;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PriceEngine.Dtos;

namespace PriceEngine.Controllers
{
    /// <summary>
    ///  Developer: Dror Kanfi
    ///  Price Engine Controller 
    /// </summary>
    /// 

    public class PriceEngController : BaseApiController
    {
        private readonly IPriceEngineService _PriceEngineService;

        public PriceEngController(IPriceEngineService PriceEngineService)
        {
            _PriceEngineService = PriceEngineService;
        }


        [HttpGet("GetAllPrice")]
        public async Task<IActionResult> GetAllPrice([FromQuery] getAllPriceRequestDto dto)
        {
            try
            {
                return Ok(await _PriceEngineService.GetAllPriceResultAsync());
            }
            catch (Exception ex)
            {
                var res = new
                {
                    returnCode = 0,
                    returnDescription = "Exception Details: " + ex.ToString()
                };

                return StatusCode(StatusCodes.Status500InternalServerError, res);

            }


        }

        [HttpGet("GetAllPriceWithNotes")]
        public async Task<IActionResult> GetAllPriceWithNotes([FromQuery] getAllPriceRequestDto dto)
        {
            try
            {
                return Ok(await _PriceEngineService.GetAllPriceResultWithNotesAsync());
            }
            catch (Exception ex)
            {
                var res = new
                {
                    returnCode = 0,
                    returnDescription = "Exception Details: " + ex.ToString()
                };

                return StatusCode(StatusCodes.Status500InternalServerError, res);

            }


        }

        [HttpGet("GetPrice")]
        public async Task<IActionResult> getPrice([FromQuery] getPriceRequestDto dto)
        {
            try
            {
                var req = new getPriceRequest
                {
                    RequestId = dto.RequestId,
                    From_Station_Code_ISR = dto.From_Station_Code_ISR,
                    To_Station_Code_ISR = dto.To_Station_Code_ISR,
                    Profile_ID = dto.Profile_ID
                };
                return Ok(await _PriceEngineService.GetPriceResultAsync(req));
            }

            catch (Exception ex)

            {
                var res = new
                {
                    returnCode = 0,
                    returnDescription = "Exception Details: " + ex.ToString()
                };
                return StatusCode(StatusCodes.Status500InternalServerError, res);
            }
        }



        [HttpGet("GetProfiles")]
        public async Task<IActionResult> getProfiles([FromQuery] getProfilesDto dto)
        {
            try
            {
                var req = new getProfiles
                {
                    RequestId = dto.RequestId,
                    SystemType=dto.SystemType
                    // LangId=dto.LangId                  
                };
                return Ok(await _PriceEngineService.getProfilesResultAsync(req));
            }

            catch (Exception ex)

            {
                var res = new
                {
                    returnCode = 0,
                    returnDescription = "Exception Details: " + ex.ToString()
                };

                return StatusCode(StatusCodes.Status500InternalServerError, res);

            }


        }

    }



}
