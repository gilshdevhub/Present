using AutoMapper;
using Core.Entities;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;
using System.Text.Json;

namespace SiteManagement.Controllers;

public class TenderTypesController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly ITenderTypesService _tenderTypesService;

    public TenderTypesController(ITenderTypesService tenderTypesService, IMapper mapper)
    {
        _mapper = mapper;
        _tenderTypesService = tenderTypesService;
    }

    [HttpGet]
   [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<TenderTypes>> GetAppTenderTypes()
    {
        IEnumerable<TenderTypes> tenderTypes = await _tenderTypesService.GetTenderTypesAsync();
        return tenderTypes;
    }

    [HttpGet("{id}")]
   [Authorize(Policy = "PageRole")]
    public async Task<TenderTypes> GetAppTenderTypesById(int id)
    {
        TenderTypes tenderType = await _tenderTypesService.GetTenderTypeByIdAsync(id);
        return tenderType;
    }

    [HttpPost]
   [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<TendersTypesPostDto>> AddTenderTypes(TendersTypesPostDto request)
    {
        ActionResult<TendersTypesPostDto> result;

        TenderTypes requseEnt = _mapper.Map<TenderTypes>(request);

        try
        {
            TenderTypes responseENT = await _tenderTypesService.AddTenderTypesAsync(requseEnt);
            TendersTypesPostDto res = JsonSerializer.Deserialize<TendersTypesPostDto>(JsonSerializer.Serialize(responseENT));
            result = Ok(res);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpPut]
   [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<TendersTypesPutDto>> UpdateTenderTypes(TendersTypesPutDto request)
    {
        ActionResult<TendersTypesPutDto> result;

        TenderTypes requseEnt = _mapper.Map<TenderTypes>(request);

        try
        {
            TenderTypes responseENT = await _tenderTypesService.UpdateTenderTypesAsync(requseEnt);
            TendersTypesPutDto res =  JsonSerializer.Deserialize<TendersTypesPutDto>(JsonSerializer.Serialize(responseENT));
            result = Ok(res);
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpDelete("{id}")]
   [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteTenderTypes(int id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _tenderTypesService.DeleteTenderTypesAsync(id);
            if (res)
            {
                result = Ok(res);
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
