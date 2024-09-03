using AutoMapper;
using Core.Entities.PriceEngine;
using Core.Enums;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteManagement.Controllers;

public class PriceNotesController : BaseApiController
{
    private readonly IPriceEngineService _priceEngineService;
    private readonly IMapper _mapper;

    public PriceNotesController(IPriceEngineService priceEngineService, IMapper mapper)
    {
        _priceEngineService = priceEngineService;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<PriceNotesResponseDto>> GetAllPriceNotesAsync()
    {
        IEnumerable<PriceNotesResponseDto> priceNotes = await _priceEngineService.GetAllPriceNotesAsync();
        return priceNotes;
    }

    [HttpGet("getProfiles")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<profileDetails>>> GetProfiles()
    {
                                                             
        return Ok(await _priceEngineService.getProfilesResultAsync(new getProfiles()
        {
            RequestId = 2,
            SystemType = SystemTypes.Web
        }));
       
    }

                  
   
                           
      
    [Authorize(Policy = "PageRole")]
    [HttpPost]
    public async Task<ActionResult<PriceNotes>> AddPriceNotes(PriceNotes priceNote)
    {
        ActionResult<PriceNotes> result;

        PriceNotes priceNoteToSave = _mapper.Map<PriceNotes>(priceNote);

        try
        {
            PriceNotes newPriceNotes = await _priceEngineService.AddPriceNoteAsync(priceNoteToSave);
            if(newPriceNotes == null) 
            {
                return UnprocessableEntity(new ApiErrorResponse { Message = "הערה לפרופיל הנוכחי כבר קיימת", StatusCode = 422 });
            }
                    result = Ok(newPriceNotes);
        }
        catch(Exception ex) 
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdatePriceNotes(PriceNotes priceNote)
    {
        ActionResult result;

        try
        {
            bool success = await _priceEngineService.UpdatePriceNoteAsync(priceNote);
            result = Ok(success);
            return result;
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpDelete("{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deletePriceNotes(int Id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _priceEngineService.DeletePriceNoteAsync(Id);
            if (res)
            {
                result = Ok(true);
            }
            else
            {
                result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
            }
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }

        return result;
    }
}
