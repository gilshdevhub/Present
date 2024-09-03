using AutoMapper;
using Core.Entities.Forms;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteManagement.Controllers;

public class FormsAPIController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IAppFormsService _appFormsService;

    public FormsAPIController(IAppFormsService appFormsService, IMapper mapper)
    {
        _mapper = mapper;
        _appFormsService = appFormsService;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<FormsIdThrees>> GetThrees()
    {
        IEnumerable<FormsIdThrees> threes = await _appFormsService.GetThreesAsync();
        return threes;
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<FormsIdThrees>> AddFormsIdThrees(FormsIdThrees request)
    {
        ActionResult<FormsIdThrees> result;

        try
        {
            FormsIdThrees newThrees = await _appFormsService.AddFormsIdThreeAsync(request);
            result = Ok(newThrees);
        }
        catch (Exception ex)
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> updateFormsIdThrees(FormsIdThrees request)
    {
        ActionResult<bool> result;
              
        try
        {
            bool newFormsIdThrees = await _appFormsService.UpdateFormsIdThreesAsync(request);
                    result = Ok(newFormsIdThrees);
        }
        catch(Exception ex) 
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        return result;
    }
    [HttpDelete("{Id:int}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> deleteFormsIdThrees(int Id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _appFormsService.DeleteFormsIdThreeAsync(Id);
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
