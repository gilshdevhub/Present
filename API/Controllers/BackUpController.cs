using API.Dtos;
using API.Dtos.Configurations;
using API.Dtos.PopupMessages;
using API.Dtos.Push;
using Asp.Versioning;
using AutoMapper;
using Core.Entities;
using Core.Entities.AppMessages;
using Core.Entities.Configuration;
using Core.Entities.Push;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Filters;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;
[ApiVersion("0.9", Deprecated = true)]
[ServiceFilter(typeof(WriteToLogFilterAttribute))]
public class BackUpController : BaseApiController
{
    private readonly IBackUpService _backupService;
    private readonly IMapper _mapper;
    private readonly IConfigurationService _configurationService;
    private readonly IPopUpMessagesService _popupMessageService;
    private readonly IStationImage _stationImageService;
    private readonly IStationsService _stationsService;
    private readonly ITendersService _tendersService;
    private readonly ISingleSupplierIService _singleSupplierService;
    private readonly ITendersCommonService _tendersCommonService;
    private readonly IPlanningAndRatesService _planningAndRatesService;
    private readonly IMeetingsService _meetingsService;
    private readonly IMailingListService _mailingListService;
    private readonly IExemptionNotices _exemptionNotices;



    public BackUpController(IBackUpService backupService, IMapper mapper, IConfigurationService configurationService, 
        IPopUpMessagesService popupMessageService, IStationImage stationImageService, IStationsService stationsService,
        ITendersService tendersService, ISingleSupplierIService singleSupplierService, ITendersCommonService tendersCommonService,
        IPlanningAndRatesService planningAndRatesService, IMeetingsService meetingsService, IMailingListService mailingListService, 
        IExemptionNotices exemptionNotices)
    {
        _backupService = backupService;
        _mapper = mapper;
        _configurationService = configurationService;
        _popupMessageService = popupMessageService;
        _stationImageService = stationImageService;
        _stationsService = stationsService;
        _tendersService = tendersService;
        _singleSupplierService = singleSupplierService;
        _tendersCommonService = tendersCommonService;
        _planningAndRatesService = planningAndRatesService;
        _meetingsService = meetingsService;
        _mailingListService = mailingListService;
        _exemptionNotices = exemptionNotices;
    }

    [HttpGet("GetAllData")]
    public async Task<ActionResult<BackUpDto>> GetallData()
    {
        BackUpDto backUpDto = new BackUpDto();
        #region configuration
        IEnumerable<Configuration> configurations = await _configurationService.GetConfigurationsBySystemTypeAsync(SystemTypes.Web).ConfigureAwait(false);
        backUpDto.configurationResponses = _mapper.Map<IEnumerable<Configuration>, IEnumerable<ConfigurationResponseDto>>(configurations);
        #endregion configuration

        #region popUpMessages
        backUpDto.popUpMessages = await _popupMessageService.GetMessagesAsync().ConfigureAwait(false);
        #endregion popUpMessages

        #region pushNotifications
        backUpDto.pushNotifications = new PushDto();
        IEnumerable<PushNotificationsByWeekDay> pushNotificationsByWeekDay = await _backupService.GetPushNotificationsByWeekDay().ConfigureAwait(false);
        IEnumerable<PushByWeekDayDto> pushNotificationsByWeekDayDto = _mapper.Map<IEnumerable<PushByWeekDayDto>>(pushNotificationsByWeekDay);
        backUpDto.pushNotifications.pushNotificationsByWeekDay = pushNotificationsByWeekDayDto;

        IEnumerable<PushNotificationsByDate> pushNotificationsByDate = await _backupService.GetPushNotificationsByDate().ConfigureAwait(false);
        IEnumerable<PushByDateDto> pushNotificationsByDateDto = _mapper.Map<IEnumerable<PushByDateDto>>(pushNotificationsByDate);
        backUpDto.pushNotifications.pushNotificationsByDate = pushNotificationsByDateDto;

        #endregion pushNotifications

        #region stationSVG
        backUpDto.stationSVG = await _stationImageService.GetStationSVGPerLanguageAsync().ConfigureAwait(false);
        #endregion stationSVG

        #region stations
        IEnumerable<Station> stations = await _stationsService.GetStationsNoCache().ConfigureAwait(false);
        backUpDto.stationResponseDto = _mapper.Map<IEnumerable<StationAllDto>>(stations);
        #endregion stations

        #region stationInformation
        backUpDto.stationInformation = await _backupService.GetAllStationInfo().ConfigureAwait(false);
        #endregion stationInformation

        #region tenders
        backUpDto.tenders = await _tendersService.GetTendersAsync();
        #endregion tenders

        #region singleSupplier
        backUpDto.singleSupplier = await _singleSupplierService.GetSuppliersAsync();
        #endregion singleSupplier

        #region tenderDocuments
        backUpDto.tenderDocuments = await _tendersCommonService.GetTenderDocumentsContentAsync();
        #endregion tenderDocuments

        #region planningAndRates
        backUpDto.planningAndRates = await _planningAndRatesService.GetPlanningAndRatesAsync();
        #endregion planningAndRates

        #region meetings
        backUpDto.meetings = JsonSerializer.Deserialize<IEnumerable<MeetingsDto>>(JsonSerializer.Serialize(await _meetingsService.GetMeetingsAsync()));
        #endregion meetings

        #region mailingLists
        backUpDto.mailingLists = await _mailingListService.GetMailingListAsync();
        #endregion mailingLists

        #region exemptionNotices
        backUpDto.exemptionNotices = await _exemptionNotices.GetExemptionNoticesAsync();
        #endregion exemptionNotices

        #region translations
        backUpDto.translations = await _backupService.GetTranslations();
        #endregion translations

        #region urlTranslation
        backUpDto.urlTranslation = await _backupService.GetUrlTranslations();
        #endregion urlTranslation


        return Ok(backUpDto);
    }
}
