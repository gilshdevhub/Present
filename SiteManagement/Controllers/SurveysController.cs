using SiteManagement.Dtos;
using AutoMapper;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Core.Entities.Surveys;

namespace SiteManagement.Controllers;

public class SurveysController : BaseApiController
{
    private readonly ISurveysService _iSurveysService;
    private readonly IMapper _mapper;
    public SurveysController(ISurveysService iSurveysService, IMapper mapper)
    {
        _iSurveysService = iSurveysService;
        _mapper = mapper;
    }

       [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<SurveysDataDto>>> GetSurveys()
    {
        IEnumerable<SurveysData> surveysData = await _iSurveysService.GetSurveysAsync().ConfigureAwait(false);
        IEnumerable<SurveysDataDto> _surveysDataDto = _mapper.Map<IEnumerable<SurveysData>, IEnumerable<SurveysDataDto>>(surveysData);
        return Ok(_surveysDataDto);
    }


       [HttpGet]
    [Authorize(Policy = "PageRole")]
    [Route("SurveysResultsByDate")]
    public async Task<ActionResult<IEnumerable<SurveysResultsDto>>> GetSurveysResultsByDate(DateTime startDate, DateTime endDate, int systemTypeId)
    {
        IEnumerable<SurveysResults> surveysResults = await _iSurveysService.GetSurveysResultsByDateAsync(startDate, endDate, systemTypeId).ConfigureAwait(false);
        IEnumerable<SurveysResultsDto> _surveysResultsDto = null;
        _surveysResultsDto = (_mapper.Map<IEnumerable<SurveysResults>, IEnumerable<SurveysResultsDto>>(surveysResults));
        return _surveysResultsDto == null ? NotFound(new ApiErrorResponse((int)HttpStatusCode.NotFound, $"Surveys at between dates {startDate} {endDate} not found")) : Ok(_surveysResultsDto);
    }

    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> AddSurvey(SurveysDataDto request)
    {
        ActionResult<bool> result;

        SurveysData surveysData = _mapper.Map<SurveysData>(request);

        try
        {
            SurveysData res = await _iSurveysService.PostSurveysAsync(surveysData);

            result = Ok(res);
        }
        catch
        {
            result = BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
        }

        return result;
    }

    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdateSurvey(SurveysDataDto request)
    {
        ActionResult<bool> result;

        SurveysData surveysData = _mapper.Map<SurveysData>(request);

        try
        {
            var res = await _iSurveysService.UpdateSurveysDataAsync(surveysData);
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
    public async Task<ActionResult<bool>> deleteTrainWarning(int id)
    {
        var error = new ClientErrorData();
        ActionResult<bool> result;
        try
        {
            bool res = await _iSurveysService.DeleteSurveysDataAsync(id);
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
