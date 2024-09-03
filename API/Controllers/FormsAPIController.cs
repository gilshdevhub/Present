using API.Dtos.Forms;
using AutoMapper;
using Core.Entities.Forms;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Core.Errors;

namespace API.Controllers;
public class FormsAPIController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IAppFormsService _appFormsService;

    public FormsAPIController(IAppFormsService appFormsService, IMapper mapper)
    {
        _mapper = mapper;
        _appFormsService = appFormsService;
    }

    [HttpPost("PostForms")]
    public async Task<ActionResult<FormsResponse>> PostForms([FromForm] FullFormsDto fullForms)
    {
                             FullForms _fullForms = _mapper.Map<FullForms>(fullForms);
        try
        {
            var response = await _appFormsService.PostFormsAsync(_fullForms).ConfigureAwait(false);
            if (string.IsNullOrEmpty(response.code))
                return BadRequest(new ApiErrorResponse { Message = response.guid, StatusCode = int.Parse(response.code) });
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    [HttpPost("PostFormsAspx")]
    public async Task<ActionResult<object>> PostFormsAspx([FromForm] FullFormsDto fullForms)
    {
                             FullForms _fullForms = _mapper.Map<FullForms>(fullForms);
                          var response = await _appFormsService.PostFormsAspxAsync(_fullForms).ConfigureAwait(false);
                      object g = new
        {
            HTMLCode= response
        };
            return Ok(g);
                                       }
}
