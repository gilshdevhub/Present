using AutoMapper;
using Core.Entities.Messenger;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AccompanyingDisabled.Dtos;
using System.Text.Json;
using FirebaseAdmin;

namespace AccompanyingDisabled.Controllers;

public class AccompanyingDisabledController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IHttpClientService _httpClientService;

    public AccompanyingDisabledController(IMapper mapper, IHttpClientService httpClientService)
    {
        _mapper = mapper;
        _httpClientService = httpClientService;
    }
    [HttpPost("NewRequestForEscort")]
    public async Task<ActionResult<NewResponsetForEscortDto>> NewRequestForEscort(NewRequestForEscortDto newRequestForEscortDto)
    {
               NewResponsetForEscortDto result = new()
        {
            RequestStatus = 1,
            StatusDiscription = "אושר"
        };
        return Ok(result);
    }

    [HttpPost("CustomerResponse")]
    public async Task<ActionResult<CustomerResponseDto>> CustomerResponse(CustomerRequestDto customerRequestDto)
    {
        CustomerResponseDto result =new()
        {
            RequestStatus = 1,
            StatusDiscription = "אושר",
            StatusCode = "guid",
            Data = JsonSerializer.Serialize(customerRequestDto)
        };
        return Ok(result);
    }

    [HttpPost("GetEscortResponse")]
    public async Task<ActionResult<GetEscortLuzResponseDto>> GetEscortResponse(GetEscortLuzRequestDto escortLuzRequestDto)
    {
        GetEscortLuzResponseDto result =new()
        {
            RequestStatus = 1,
            StatusDiscription = "התקבל",
            EscortsData = new List<EscortData>()
            {
                new EscortData()
                {
                    Body="message body",
                    MessageGuid=Guid.NewGuid().ToString(),
                    StatusId=11,
                    Subject="subject text",
                    TrainDate=DateTime.Now,
                    TrainNum=123,
                    UserId=5000
                },
                 new EscortData()
                {
                    Body="message body",
                    MessageGuid=Guid.NewGuid().ToString(),
                    StatusId=11,
                    Subject="subject text",
                    TrainDate=DateTime.Now,
                    TrainNum=321,
                    UserId=6000
                }
            }
        };
        return Ok(result);
    }
    [HttpPost("EscortResponse")]
    public async Task<ActionResult<StewardOrInspectorResponseDto>> EscortResponse(StewardOrInspectorRequestDto stewardOrInspectorRequestDto)
    {
        StewardOrInspectorResponseDto result =  new ()
        {
            RequestStatus = 1,
            StatusDiscription = "התקבל"
        };
        return Ok(result);
    }


    [HttpPost("AttachFilesToCase")]
    public async Task<ActionResult<AttachFilesToCaseResnonseDto>> AttachFilesToCaseResponse(AttachFilesToCaseRequestDto attachFilesToCaseRequestDto)
    {
        AttachFilesToCaseResnonseDto result = new ()
        {
            IsSuccess = true,
            ErrorMessage = null,
            ErrorCode = 0
        };
        return Ok(result);
    }

    [HttpPost("CreateCase")]
    public async Task<ActionResult<CreateCaseResnonseDto>> CreateCaseResponse(CreateCaseRequestDto createCaseRequestDto)
    {
        CreateCaseResnonseDto result = new ()
        {
            IsSuccess = true,
            ErrorMessage = null,
            ErrorCode = 0,
            InicidendId = "00000000-0000-0000-0000-000000000000",
            CaseNumber = null
        };
        return Ok(result);
    }


    [HttpGet("GetStatus")]
    public async Task<ActionResult<DisabledTypeResponseDto>> GetStatus()
    {
        DisabledTypeResponseDto result = new()
        {
            DisabledTypes = new List<DisabledType>()
            {
                new DisabledType()
                {
                    Id = 0,
                    Type = "מקל הליכה"
                },
                new DisabledType()
                {
                    Id = 1,
                    Type = "הליכון"
                },
                new DisabledType()
                {
                    Id = 2,
                    Type = "קביים"
                },
                new DisabledType()
                {
                    Id = 3,
                    Type = "כסא גלגלים"
                },
                new DisabledType()
                {
                    Id = 4,
                    Type = "כסא גלגלים ממונע"
                },
                new DisabledType()
                {
                    Id = 5,
                    Type = "קלנועית"
                },
                new DisabledType()
                {
                    Id = 6,
                    Type = "לקות ראייה"
                },
                new DisabledType()
                {
                    Id = 7,
                    Type = "עיוורון"
                },
                new DisabledType()
                {
                    Id = 8,
                    Type = "לקות שמיעה"
                },
                new DisabledType()
                {
                    Id = 9,
                    Type = "מגבלה קוגנטיבית"
                },
                new DisabledType()
                {
                    Id = 10,
                    Type = "סוג אחר"
                }
            }
        };
        return Ok(result);

    }

}
