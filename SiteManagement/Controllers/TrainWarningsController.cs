using AutoMapper;
using Core.Entities.AppMessages;
using Core.Errors;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteManagement.Dtos;

namespace SiteManagement.Controllers;

public class TrainWarningsController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly ITrainWarningsService _trainWarningsService;

    public TrainWarningsController(ITrainWarningsService trainWarningsService, IMapper mapper)
    {
        _mapper = mapper;
        _trainWarningsService = trainWarningsService;
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<TrainWarningResponseDto>> GetAppTrainWarnings()
    {
        IEnumerable<TrainWarning> appTrainWarnings = await _trainWarningsService.GetTrainWarningsAsync();
        IEnumerable<TrainWarningResponseDto> trainWarningsDto = _mapper.Map<IEnumerable<TrainWarningResponseDto>>(appTrainWarnings);
        return trainWarningsDto;
    }

    [HttpGet("warningtypes")]
    [Authorize(Policy = "PageRole")]
    public async Task<IEnumerable<WarningType>> GetPageTypes()
    {
        return await _trainWarningsService.GetWarningTypesAsync();
    }


    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> AddTrainWarning(TrainWarningRequestDto request)
    {
        ActionResult<bool> result;

        TrainWarning trainWarning = _mapper.Map<TrainWarning>(request);

        try
        {
            bool res = await _trainWarningsService.AddTrainWarningAsync(trainWarning);

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
    public async Task<ActionResult<bool>> UpdateTrainWarning(TrainWarningRequestDto request)
    {
        ActionResult<bool> result;

        TrainWarning trainWarning = _mapper.Map<TrainWarning>(request);

        try
        {
            bool res = await _trainWarningsService.UpdateTrainWarningAsync(trainWarning);
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
            bool res = await _trainWarningsService.DeleteTrainWarningAsync(id);
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
