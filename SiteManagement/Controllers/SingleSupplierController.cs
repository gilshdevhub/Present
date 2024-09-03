using AutoMapper;
using Core.Entities;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class SingleSupplierController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly ISingleSupplierIService _singleSupplierIService;

    public SingleSupplierController( IMapper mapper, ISingleSupplierIService singleSupplierIService)
    {
        _mapper = mapper;
        _singleSupplierIService = singleSupplierIService;
    }

    [HttpGet("{categoryId}")]
   [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<SingleSupplierDto>> GetSingleSuppliers(int categoryId)
    {
        IEnumerable<SingleSupplierDto> singleSupplier = await _singleSupplierIService.GetSuppliersNoCache(categoryId);
        return singleSupplier;
    }
    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<SingleSupplierDto>> GetSingleSuppliers()
    {
        IEnumerable<SingleSupplierDto> singleSupplier = await _singleSupplierIService.GetSuppliersAsync();
        return singleSupplier;
    }
                     
    [HttpPost]
   [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<Guid>> AddSingleSupplier(SingleSupplierPostDto request)
    {
        ActionResult<Guid> result;

        SingleSupplier requseEnt = _mapper.Map<SingleSupplier>(request);

        try
        {
            SingleSupplier responseENT = await _singleSupplierIService.AddSupplierAsync(requseEnt);
                     result = Ok(responseENT.Id);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpPut]
   [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<SingleSupplierPutDto>> UpdateSingleSupplier(SingleSupplierPutDto request)
    {
        ActionResult<SingleSupplierPutDto> result;
        SingleSupplier requseEnt = _mapper.Map<SingleSupplier>(request);

        try
        {
            SingleSupplier responseENT = await _singleSupplierIService.UpdateSupplierAsync(requseEnt);
            SingleSupplierPutDto res =  _mapper.Map<SingleSupplierPutDto>(responseENT);
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
    public async Task<ActionResult<bool>> deleteTenders(Guid id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _singleSupplierIService.DeleteSupplierAsync(id);
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
