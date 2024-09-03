using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Asp.Versioning;
using Core.Filters;

namespace API.Controllers;
[ApiVersion("0.9", Deprecated = true)]
[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class PaymantsController : BaseApiController
{
    private readonly IPaymantsService _paymantsService;
    private readonly IKirtusConnetctionService _kirtusConnetctionService;
    private readonly IMapper _mapper;

    public PaymantsController(IPaymantsService paymantsService, IMapper mapper, IKirtusConnetctionService kirtusConnetctionService)
    {
        _paymantsService = paymantsService;
        _kirtusConnetctionService = kirtusConnetctionService;
        _mapper = mapper;
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LoginResponseList>> Login(LoginShortRequestDto requestData)
    {
        LoginResponseList kirtusResult = await _kirtusConnetctionService.Login(requestData);
               return Ok(kirtusResult);
    }

    [HttpPost("TxnSetup")]
    public async Task<ActionResult<Object>> TxnSetup(TxnInternalRequestDto txnSetupRequestDto)    {
        var result = await _paymantsService.TxnSetup(txnSetupRequestDto).ConfigureAwait(false);
        return Ok(JsonSerializer.Deserialize<Object>(JsonSerializer.Serialize(result)));
    }

    [HttpPost("InquireTrnsaction")]
    public async Task<ActionResult<Row>> TxnInternal(InquireTransactionsRequestDto requestData)
    {

          string result = await _paymantsService.InquireTransaction (requestData).ConfigureAwait(false);
        return Ok(result);
                                                                              
   
}
   

}
