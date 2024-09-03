using API.Dtos.Compensations;
using Asp.Versioning;
using AutoMapper;
using Core.Entities.Compensation;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace API.Controllers;
[ApiVersion("0.9", Deprecated = true)]
[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class CompensationsController : BaseApiController
{
    private readonly ICompensationService _compensationService;
    private readonly IMapper _mapper;
    private readonly IJwtHandlerService _jwtHandlerService;

    public CompensationsController(ICompensationService compensationService, IMapper mapper, IJwtHandlerService jwtHandlerService)
    {
        _compensationService = compensationService;
        _mapper = mapper;
        _jwtHandlerService = jwtHandlerService;
    }

    [HttpPost]
    public async Task<ActionResult<CompensationResponseDto>> CreateCompensation(CompensationRequestDto compensationDto)
    {
        Compensation compensationReq = _mapper.Map<Compensation>(compensationDto);
        Compensation compensation = await _compensationService.CreateCompensationAsync(compensationReq).ConfigureAwait(false);
        CompensationResponseDto compensationResponseDto = _mapper.Map<CompensationResponseDto>(compensation);
        return Ok(compensationResponseDto);
    }

    [HttpGet]
    public async Task<ActionResult<CompensationSearchResponsetDto>> SearchCompensations([FromQuery] CompensationSearchRequestDto compensationSearchRequesttDto)
    {
        SearchCompensation searchCompensation = _mapper.Map<SearchCompensation>(compensationSearchRequesttDto);
        IEnumerable<Compensation> compensations = await _compensationService.SearchCompensationsAsync(searchCompensation).ConfigureAwait(false);
        IEnumerable<CompensationSearchResult> compensationSearchResults = _mapper.Map<IEnumerable<CompensationSearchResult>>(compensations);
        CompensationSearchResponsetDto compensationSearchResponsetDto = new() { CompensationsCard = compensationSearchResults };
        return Ok(compensationSearchResponsetDto);
    }

    [HttpPost("otp")]
    public async Task<ActionResult<CompensationOtpResponse>> CreateOtp(CompensationOtpRequestDto compensationOtpDto)
    {
        CompensationOtpRequest compensationOtpRequest = _mapper.Map<CompensationOtpRequest>(compensationOtpDto);
        CompensationOtpResponse response = await _compensationService.CreateOtpAsync(compensationOtpRequest).ConfigureAwait(false);
        return response == null ?
            StatusCode((int)HttpStatusCode.InternalServerError, new ApiErrorResponse { Message = "otp creation failed", StatusCode = 500 }) : Ok(response);
    }

    [HttpGet("otp/validate")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public ActionResult ValidateToken([FromBody] string token)
    {
        if (_jwtHandlerService.ValidateToken(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            var claims = new
            {
                PhoneNumber = jwtToken.Claims.First(claim => claim.Type == "PhoneNumber").Value,
                Otp = jwtToken.Claims.First(claim => claim.Type == "Otp").Value
            };

            return Ok(claims);
        }

        return BadRequest(new ApiErrorResponse { Message = "token is invalid", StatusCode = 400 });
    }

    [HttpPost("otp/gettickets")]
    [Authorize]
    public async Task<ActionResult<CompensationSearchResponsetDto>> GetCompensations(CompensationOtpSearchRequestDto compensationOtpRequestDto)
    {
        string otp = HttpContext.User.Claims.SingleOrDefault(p => p.Type == "Otp").Value;
        string phoneNum = HttpContext.User.Claims.SingleOrDefault(p => p.Type == "PhoneNumber").Value;

        if (compensationOtpRequestDto.PhoneNumber != phoneNum || compensationOtpRequestDto.Otp != otp)
        {
            return BadRequest(new ApiErrorResponse { Message = "לא נמצאה התאמה", StatusCode = 400 });
        }

        IEnumerable<Compensation> compensations = await _compensationService.SearchOtpCompensationsAsync(new SearchCompensation { PhoneNumber = phoneNum }).ConfigureAwait(false);
        IEnumerable<CompensationSearchResult> compensationSearchResults = _mapper.Map<IEnumerable<CompensationSearchResult>>(compensations);
        CompensationSearchResponsetDto compensationSearchResponsetDto = new() { CompensationsCard = compensationSearchResults };
        return Ok(compensationSearchResponsetDto);
    }
}
