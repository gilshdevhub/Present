using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

public class TendersController : BaseApiController
{
    private readonly ITendersService _tendersService;
    private readonly ITenderTypesService _tenderTypesService;
    private readonly IMailingListService _mailingListService;
    private readonly ISingleSupplierIService _singleSupplierService;
    private readonly IPlanningAndRatesService _planningAndRatesService;
    private readonly IMeetingsService _meetingsService;
    private readonly IExemptionNotices _exemptionNotices;
    private readonly IMapper _mapper;
    private readonly ITendersCommonService _tendersCommonService;
    private readonly ISearchService _searchService;

    public TendersController(ITendersService tendersService, ITenderTypesService tenderTypesService,
        IMailingListService mailingListService, ISingleSupplierIService singleSupplierService,
        IMeetingsService meetingsService, IExemptionNotices exemptionNotices, IMapper mapper,
        IPlanningAndRatesService planningAndRatesService, ITendersCommonService tendersCommonService, ISearchService searchService)
    {
        _tendersService = tendersService;
        _tenderTypesService = tenderTypesService;
        _mailingListService = mailingListService;
        _singleSupplierService = singleSupplierService;
        _meetingsService = meetingsService;
        _exemptionNotices = exemptionNotices;
        _mapper = mapper;
        _planningAndRatesService = planningAndRatesService;
        _tendersCommonService = tendersCommonService;
        _searchService = searchService;
    }

    [HttpGet("GetTenders")]
    public async Task<IEnumerable<TendersDto>> GetTenders()
    {
        IEnumerable<TendersDto> tenders = await _tendersService.GetTendersAsync();
        return tenders;
    }
    [HttpGet("GetTendersByType")]
    public async Task<IEnumerable<TendersDto>> GetTendersByType(int type)
    {
        IEnumerable<TendersDto> tenders = await _tendersService.GetTendersByTypeAsync(type);

        return tenders;
    }
    [HttpGet("GetTenderById")]
    public async Task<TendersDto> GetTenderById(Guid id)
    {
        TendersDto tender = await _tendersService.GetTenderByIdAsync(id);
        return tender;
    }


    [HttpGet("GetTenderTypes")]
    public async Task<IEnumerable<TenderTypes>> GetTenderTypes()
    {
        IEnumerable<TenderTypes> tenderTypes = await _tenderTypesService.GetTenderTypesAsync();
        return tenderTypes;
    }
    [HttpGet("GetTenderTypeById")]
    public async Task<TenderTypes> GetTenderTypeById(int id)
    {
        TenderTypes tenderType = await _tenderTypesService.GetTenderTypeByIdAsync(id);
        return tenderType;
    }


    [HttpGet("GetSingleSupplier")]
    public async Task<IEnumerable<SingleSupplierDto>> GetSingleSupplier()
    {
        IEnumerable<SingleSupplierDto> singleSupplier = await _singleSupplierService.GetSuppliersAsync();
        return singleSupplier;
    }

    [HttpGet("GetTenderDocuments")]
    public async Task<IEnumerable<TenderDocuments>> GetTenderDocuments()
    {
        IEnumerable<TenderDocuments> tenderDocuments = await _tendersCommonService.GetTenderDocumentsContentAsync();
        return tenderDocuments;
    }

    [HttpGet("GetSingleSupplierById")]
    public async Task<SingleSupplierDto> GetSingleSupplierById(Guid id)
    {
        SingleSupplierDto singleSupplier = await _singleSupplierService.GetSupplierByIdAsync(id);
        return singleSupplier;
    }


    [HttpGet("GetPlanningAndRates")]
    public async Task<IEnumerable<PlanningAndRatesDto>> GetPlanningAndRates()
    {
        IEnumerable<PlanningAndRatesDto> singleSupplier = await _planningAndRatesService.GetPlanningAndRatesAsync();
        return singleSupplier;
    }
    [HttpGet("GetPlanningAndRatesById")]
    public async Task<PlanningAndRatesDto> GetPlanningAndRatesById(Guid id)
    {
        PlanningAndRatesDto singleSupplier = await _planningAndRatesService.GetPlanningAndRatesByIdAsync(id);
        return singleSupplier;
    }

    [HttpGet("GetMeetings")]
    public async Task<IEnumerable<MeetingsDto>> GetMeetings()
    {
        IEnumerable<MeetingsDto> meetings =                                            JsonSerializer.Deserialize<IEnumerable< MeetingsDto>>(JsonSerializer.Serialize(await _meetingsService.GetMeetingsAsync()));
        return meetings;
    }
    [HttpGet("GetMeetingById")]
    public async Task<MeetingsDto> GetMeetingById(int id)
    {
        MeetingsDto meeting =                                JsonSerializer.Deserialize<MeetingsDto>(JsonSerializer.Serialize(await _meetingsService.GetMeetingByIdAsync(id)));
        return meeting;
    }


    [HttpGet("GetMailingLists")]
    public async Task<IEnumerable<MailingList>> GetMailingLists()
    {
        IEnumerable<MailingList> mailingLists = await _mailingListService.GetMailingListAsync();
        return mailingLists;
    }
    [HttpGet("GetMailingListById")]
    public async Task<MailingList> GetMailingListById(int id)
    {
        MailingList mailingList = await _mailingListService.GetMailingListByIdAsync(id);
        return mailingList;
    }

    [HttpGet("GetExemptionNotices")]
    public async Task<IEnumerable<ExemptionNoticesDto>> GetAppExemptionNotices()
    {
        IEnumerable<ExemptionNoticesDto> tenders = await _exemptionNotices.GetExemptionNoticesAsync();
        return tenders;
    }

    [HttpGet("GetExemptionNoticeById")]
    public async Task<ExemptionNoticesDto> GetExemptionNoticeById(Guid id)
    {
        ExemptionNoticesDto exemptionNotices = await _exemptionNotices.GetExemptionNoticesByIdAsync(id);
        return exemptionNotices;
    }

    [HttpPut("RemoveSingleMailFromList")]
    public async Task<ActionResult<MailingList>> RemoveSingleMailFromList(string mail)
    {
        bool result = await _mailingListService.DeleteSingleMailAsync(mail).ConfigureAwait(false);
        if (!result)
            return BadRequest();
        return Ok(result);
    }
 
    [HttpPost("UpdateMailingList")]
    public async Task<ActionResult<MailingList>> AddMailingList(MailingListPutDto request)
    {
        ActionResult<MailingList> result;

        MailingList requseEnt = _mapper.Map<MailingList>(request);

        try
        {
            MailingList responseENT = await _mailingListService.AddMailingListAsync(requseEnt);
            MailingListPutDto res = JsonSerializer.Deserialize<MailingListPutDto>(JsonSerializer.Serialize(responseENT));
            result = Ok(responseENT);
        }
        catch (Exception ex)
        {
            var error = new ClientErrorData();
            error.Title = "failed to add a new mailingList";
            result = BadRequest(error);
        }

        return result;
    }




    [HttpGet("Search")]
    public async Task<string> SearchPages(string term, string culture = "he")
    {
       
        if (string.IsNullOrEmpty(term)) { return "No search term."; }
             try
        {
            var content = JsonSerializer.Serialize(await _searchService.Search(term, culture));
            return content;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }


    }

}
