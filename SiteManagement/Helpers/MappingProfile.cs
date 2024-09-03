using SiteManagement.Dtos;
using AutoMapper;
using Core.Entities.Identity;
using Core.Entities.Configuration;
using Core.Entities.Translation;
using Core.Entities.Vouchers;
using Core.Entities.AppMessages;
using Core.Entities.Messenger;
using Core.Entities.Push;
using Core.Entities.PagesManagement;
using Core.Entities.Surveys;
using Core.Entities.Stations;
using Core.Entities;

namespace SiteManagement.Helpers;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppUser, UserDto>()
            .ForMember(p => p.Token, src => src.MapFrom<TokenResolver>())
            .ForMember(t => t.Roles, s => s.MapFrom<RoleNameResolver>());


        CreateMap<AppRole, RoleDto>()
        .ForMember(dest => dest.PageRoleNew, src => src.MapFrom(p => p.PageRoleNew
                          .Select(q => new PageRoleNewResponse
                          {
                              PageId = q.PageId,
                              Readable = q.Readable,
                              Visible = q.Visible,
                              Updatable = q.Updatable,
                          })));

        CreateMap<UserDto, AppUser>();

        CreateMap<ConfigurationRequestDto, ConfigurationParameter>();
        CreateMap<ConfigurationParameter, ConfigurationResponseDto>();

        CreateMap<TranslationRequestDto, Translation>();
        CreateMap<Translation, TranslationResponseDto>();
       
        CreateMap<URLTranslationRequestDto, URLTranslation>();
        CreateMap<URLTranslation, URLTranslationResponseDto>()
            .ForMember(dest => dest.SystemTypeName, src => src.MapFrom(p => p.SystemType.Name));

        CreateMap<Station, StationDto>()
            .ForMember(dest => dest.Synonyms, src => src.MapFrom(p => p.Synonym.Select(q => new Synonym { LanguageId = q.LanguageId, SynonymName = q.SynonymName, Id = q.Id, StationId = q.StationId })))
            .ForMember(dest => dest.AirPolution, src => src.MapFrom(p => p.StationInfo.AirPolution))
            .ForMember(dest => dest.ParkingCosts, src => src.MapFrom(p => p.StationInfo.ParkingCosts))
            .ForMember(dest => dest.BikeParking, src => src.MapFrom(p => p.StationInfo.BikeParking))
            .ForMember(dest => dest.BikeParkingCosts, src => src.MapFrom(p => p.StationInfo.BikeParkingCosts))
            .ForMember(dest => dest.StationMap, src => src.MapFrom(p => p.StationInfo.StationMap))
            .ForMember(dest => dest.NonActiveElavators, src => src.MapFrom(p => p.StationInfo.NonActiveElavators))
            .ForMember(dest => dest.StationIsClosed, src => src.MapFrom(p => p.StationInfo.StationIsClosed))
            .ForMember(dest => dest.LinesMapsX, src => src.MapFrom(p => p.StationInfo.LinesMapsX))
            .ForMember(dest => dest.LinesMapsY, src => src.MapFrom(p => p.StationInfo.LinesMapsY))
            .ForMember(dest => dest.StationInfoFromDate, src => src.MapFrom(p => p.StationInfo.StationInfoFromDate))
            .ForMember(dest => dest.StationInfoToDate, src => src.MapFrom(p => p.StationInfo.StationInfoToDate))
            ;

        CreateMap<StationInfo, StationDto>();

        CreateMap<Station, StationGateDto>()
                             ;
        CreateMap<StationGateRequestDto, StationGate>();
        CreateMap<StationGateRequestInsertDto, StationGate>();
                                          
        CreateMap<TemplatesDto, StationActivityHoursTemplates>();
        CreateMap<TemplatesPostDto, StationActivityHoursTemplates>();
        CreateMap<StationActivityHoursTemplatesLineDto, StationActivityHoursTemplatesLine>();
        CreateMap<StationActivityHoursTemplatesLinePostDto, StationActivityHoursTemplatesLine>();
        CreateMap<HoursDto, StationGateActivityHours>();
        CreateMap<HoursPostDto, StationGateActivityHours>();
        CreateMap<StationGateActivityHoursLineDto, StationGateActivityHoursLines>();
        CreateMap<StationGateActivityHoursLinePostDto, StationGateActivityHoursLines>();

        CreateMap<StationDto, Station>()
            .ForMember(dest => dest.RjpaName, src => src.MapFrom(p => p.HebrewName))
            .ForMember(dest => dest.Synonym, src => src.MapFrom(p => p.Synonyms.Select(q => new Synonym { LanguageId = q.LanguageId, SynonymName = q.SynonymName })))
            ;

        CreateMap<StationUpdateDto, Station>()
           .ForMember(dest => dest.RjpaName, src => src.MapFrom(p => p.HebrewName))
           .ForMember(dest => dest.Synonym, src => src.MapFrom(p => p.Synonyms.Select(q => new Synonym { LanguageId = q.LanguageId, SynonymName = q.SynonymName, StationId = q.StationId, Id = q.Id })))
           ;

        CreateMap<PopUpMessagesRequestDto, PopUpMessages>();
        CreateMap<PopUpMessages, PopUpMessagesResponseDto>()
          .ForMember(dest => dest.PageTypeName, src => src.MapFrom(p => p.PageType.Name))
          .ForMember(dest => dest.SystemTypeName, src => src.MapFrom(p => p.SystemType.Name));

        CreateMap<TrainWarningRequestDto, TrainWarning>();
        CreateMap<TrainWarning, TrainWarningResponseDto>()
          .ForMember(dest => dest.WarningTypeName, src => src.MapFrom(p => p.WarningType.Name))
          .ForMember(dest => dest.SystemTypeName, src => src.MapFrom(p => p.SystemType.Name));

        CreateMap<Page, PageDto>()
          .ForMember(dest => dest.Roles, src => src.MapFrom(p => p.PageRoleNew.Select(q => q.Role.Name)));

        CreateMap<PushRouteRequestDto, PushRouteQuery>();
        CreateMap<PushRouting, PushRouteResponseDto>()
            .ForMember(dest => dest.PushRouteId, src => src.MapFrom(p => p.Id));
        CreateMap<PushNotificationInfoDto, PushNotification>().ReverseMap();
        CreateMap<PushLogRequestDto, PushLogRequest>();
        CreateMap<PushNotificationsLog, PushLogResponseDto>();

        CreateMap<SentMessageCriteriaDto, SentMessageCriteria>();
        CreateMap<SentMessageOld, SentMessageResponsetDto>();
        CreateMap<MessageInfoDto, MessageInfo>();
        CreateMap<int, TrainsDto>()
            .ForMember(dest => dest.TrainNumber, src => src.MapFrom(p => p.ToString()));

        CreateMap<int, StationIdsDto>()
          .ForMember(dest => dest.StationId, src => src.MapFrom(p => p.ToString()));

        CreateMap<SurveysData, SurveysDataDto>();
        CreateMap<SurveysDataDto, SurveysData>();
        CreateMap<IEnumerable<SurveysDataDto>, IEnumerable<SurveysData>>();

        CreateMap<SurveysResults, SurveysResultsDto>()
            .ForMember(dest => dest.StartDate, src => src.MapFrom(p => p.TimeStamp));
        CreateMap<SurveysResultsDto, SurveysResults>();
        CreateMap<IEnumerable<SurveysResultsDto>, IEnumerable<SurveysResults>>();

        CreateMap<IEnumerable<StationImageDto>, IEnumerable<StationImage>>();
        CreateMap<StationImageDto, StationImage>();
        CreateMap<StationImage, StationImageDto>();
        CreateMap<StationImage, ImageElementsDto>();

        CreateMap<TendersTypesPostDto, TenderTypes>();
        CreateMap<TendersTypesPutDto, TenderTypes>();
        CreateMap<MeetingsPostDto, Meetings>();
        CreateMap<MeetingsDto, Meetings>();
        CreateMap<MeetingsPutDto, Meetings>();
        CreateMap<MailingListPostDto, MailingList>();
        CreateMap<MailingList, MailingListPostDto>();
       
        CreateMap<TendersPostDto, Tenders>()
            .ForMember(dest => dest.Domain, src => src.MapFrom(p => p.Filed))
            .ForMember(dest => dest.Page, src => src.MapFrom(p => p.Category))
            .ForMember(dest => dest.TypeOfTender, src => src.MapFrom(p => p.Type));
        CreateMap<TendersDto, Tenders>()
            .ForMember(dest => dest.Domain, src => src.MapFrom(p => p.Filed))
            .ForMember(dest => dest.Page, src => src.MapFrom(p => p.Category))
            .ForMember(dest => dest.TypeOfTender, src => src.MapFrom(p => p.Type));

        CreateMap<Tenders, TendersPutDto>()
           .ForMember(dest => dest.Filed, src => src.MapFrom(p => p.Domain))
           .ForMember(dest => dest.Category, src => src.MapFrom(p => p.Page))
           .ForMember(dest => dest.Type, src => src.MapFrom(p => p.TypeOfTender));

        

        CreateMap<SingleSupplierPostDto, SingleSupplier>()
            .ForMember(dest => dest.Domain, src => src.MapFrom(p => p.Filed))
            .ForMember(dest => dest.Page, src => src.MapFrom(p => p.Category))
            .ForMember(dest => dest.TypeOfTender, src => src.MapFrom(p => p.Type));

        CreateMap<SingleSupplierPutDto, SingleSupplier>()
            .ForMember(dest => dest.Domain, src => src.MapFrom(p => p.Filed))
            .ForMember(dest => dest.Page, src => src.MapFrom(p => p.Category))
            .ForMember(dest => dest.TypeOfTender, src => src.MapFrom(p => p.Type));

        CreateMap<SingleSupplier, SingleSupplierPutDto>()
          .ForMember(dest => dest.Filed, src => src.MapFrom(p => p.Domain))
          .ForMember(dest => dest.Category, src => src.MapFrom(p => p.Page))
          .ForMember(dest => dest.Type, src => src.MapFrom(p => p.TypeOfTender));

        CreateMap<ExemptionNoticesPostDto, ExemptionNotices>()
            .ForMember(dest => dest.Domain, src => src.MapFrom(p => p.Filed))
            .ForMember(dest => dest.Page, src => src.MapFrom(p => p.Category))
            .ForMember(dest => dest.TypeOfTender, src => src.MapFrom(p => p.Type));
        CreateMap<ExemptionNoticesPutDto, ExemptionNotices>()
           .ForMember(dest => dest.Domain, src => src.MapFrom(p => p.Filed))
           .ForMember(dest => dest.Page, src => src.MapFrom(p => p.Category))
           .ForMember(dest => dest.TypeOfTender, src => src.MapFrom(p => p.Type));
        CreateMap<ExemptionNotices, ExemptionNoticesPutDto>()
        .ForMember(dest => dest.Filed, src => src.MapFrom(p => p.Domain))
        .ForMember(dest => dest.Category, src => src.MapFrom(p => p.Page))
        .ForMember(dest => dest.Type, src => src.MapFrom(p => p.TypeOfTender));

        CreateMap<PlanningAndRatesDto, PlanningAndRates>()
            .ForMember(dest => dest.Domain, src => src.MapFrom(p => p.Filed))
            .ForMember(dest => dest.Page, src => src.MapFrom(p => p.Category))
            .ForMember(dest => dest.TypeOfTender, src => src.MapFrom(p => p.Type));
 
        CreateMap<PlanningAndRates, PlanningAndRatesPutDto>()
        .ForMember(dest => dest.Filed, src => src.MapFrom(p => p.Domain))
        .ForMember(dest => dest.Category, src => src.MapFrom(p => p.Page))
        .ForMember(dest => dest.Type, src => src.MapFrom(p => p.TypeOfTender));
        CreateMap<Core.Entities.TenderDocumentsDto, TenderDocuments>();
    }
}
