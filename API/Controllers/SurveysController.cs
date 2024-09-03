using API.Dtos.Surveys;
using AutoMapper;
using Core.Entities.Surveys;
using Core.Enums;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

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
    [Route("GetSurveys")]
    public async Task<ActionResult<IEnumerable<SurveysDataDto>>> GetSurveys()
    {
        IEnumerable<SurveysData> surveysData = await _iSurveysService.GetSurveysAsync().ConfigureAwait(false);
        IEnumerable<SurveysDataDto> _surveysDataDto = _mapper.Map<IEnumerable<SurveysData>, IEnumerable<SurveysDataDto>>(surveysData);
        return Ok(_surveysDataDto);
    }

       [HttpGet]
    [Route("SurveysResultsBySurveyDates")]
    public async Task<ActionResult<IEnumerable<SurveysResultsDto>>> SurveysResultsBySurveyDates( int SurveyID)
    {
        IEnumerable<SurveysResults> surveysResults = await _iSurveysService.GetSurveysResultsBySurveyDateAsync(SurveyID).ConfigureAwait(false);
        IEnumerable<SurveysResultsDto> _surveysResultsDto = _mapper.Map<IEnumerable<SurveysResults>, IEnumerable<SurveysResultsDto>>(surveysResults);
        return Ok(_surveysResultsDto);
    }

       [HttpGet]
    [Route("SurveysResultsBySurveyId")]
    public async Task<ActionResult<IEnumerable<SurveysResultsDto>>> GetSurveysResultsBySurveyId( int SurveyID)
    {
        IEnumerable <SurveysResults> surveysResults = await _iSurveysService.GetSurveysResultsBySurveyIdAsync(SurveyID).ConfigureAwait(false);
        IEnumerable<SurveysResultsDto> _surveysResultsDto = (_mapper.Map< IEnumerable<SurveysResults>, IEnumerable<SurveysResultsDto>>(surveysResults, src => src.Items["SurveyId"] = SurveyID));
        return _surveysResultsDto == null ? NotFound(new ApiErrorResponse((int)HttpStatusCode.NotFound, $"Survey {SurveyID} was not found")) : Ok(_surveysResultsDto);
    }
    
       [HttpGet]
    [Route("SurveysResultsByDate")]
    public async Task<ActionResult<IEnumerable<SurveysResultsDto>>> GetSurveysResultsByDate( DateTime startDate, DateTime endDate, int systemTypeId)
    {
        IEnumerable<SurveysResults> surveysResults = await _iSurveysService.GetSurveysResultsByDateAsync(startDate, endDate, systemTypeId).ConfigureAwait(false);
        IEnumerable<SurveysResultsDto> _surveysResultsDto = null;
        _surveysResultsDto = (_mapper.Map<IEnumerable<SurveysResults>, IEnumerable<SurveysResultsDto>>(surveysResults));
        return _surveysResultsDto == null ? NotFound(new ApiErrorResponse((int)HttpStatusCode.NotFound, $"Surveys at between dates {startDate} {endDate} not found")) : Ok(_surveysResultsDto);

     }

       [HttpPost]
    [Route("PostSurveysResults")]
    public async Task<ActionResult<SurveysResultsDto>> PostSurveysResults([FromBody] SurveysResultsDto surveysResults)
    {
        ActionResult<SurveysResultsDto> result;

        SurveysResults _surveysResults = _mapper.Map<SurveysResults>(surveysResults);

        try
        {
            SurveysResults newSurveysResults = await _iSurveysService.PostSurveysResultsAsync(_surveysResults).ConfigureAwait(false);
            SurveysResultsDto surveysResultsDto = _mapper.Map<SurveysResultsDto>(newSurveysResults);
            result = Ok(surveysResultsDto);
        }
        catch
        {
            var error = new ClientErrorData();
            error.Title = "failed to surveys results";
            result = BadRequest(error);
        }

        return result;
    }

    [HttpGet]
    [Route("SurveysResultsInpt")]
    public async Task<ActionResult<SurveysResultsDto>> GetSurveysResults(DateTime timestamp,string custID,string systypeID,int score,int survyID)
    {
        SurveysResultsDto surveysResults=new SurveysResultsDto();
        surveysResults.CustID = custID;
        surveysResults.SystemTypeId = (int)(SystemTypes)Enum.Parse(typeof(SystemTypes), systypeID);
        surveysResults.TimeStamp = timestamp;
        surveysResults.Score = score;
        surveysResults.SurveyId= survyID;

        ActionResult<SurveysResultsDto> result;
        SurveysResults _surveysResults = _mapper.Map<SurveysResults>(surveysResults);
        try
        {
            SurveysResults newSurveysResults = await _iSurveysService.PostSurveysResultsAsync(_surveysResults).ConfigureAwait(false);
            SurveysResultsDto surveysResultsDto = _mapper.Map<SurveysResultsDto>(newSurveysResults);
            result = Ok(surveysResultsDto);
        }
        catch
        {
            var error = new ClientErrorData();
            error.Title = "failed to surveys results";
            result = BadRequest(error);
        }
        return result;
    }

    [HttpPost]
    [Route("PostSurveys")]
    public async Task<ActionResult<SurveysDataDto>> PostSurveys([FromBody] SurveysDataDto surveysDataDto)
    {
        ActionResult<SurveysDataDto> result;

        SurveysData _surveysData = _mapper.Map<SurveysData>(surveysDataDto);

        try
        {
            SurveysData newSurveysData = await _iSurveysService.PostSurveysAsync(_surveysData).ConfigureAwait(false);
            SurveysDataDto surveysDatasDto = _mapper.Map<SurveysDataDto>(newSurveysData);
            result = Ok(surveysDatasDto);
        }
        catch
        {
            var error = new ClientErrorData();
            error.Title = "failed to surveys results";
            result = BadRequest(error);
        }

        return result;
    }
}
