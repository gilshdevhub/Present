using API.Dtos;
using API.Dtos.Compensations;
using API.Dtos.Configurations;
using API.Dtos.Push;
using API.Dtos.PopupMessages;
using API.Dtos.RailUpdates;
using API.Dtos.Translations;
using AutoMapper;
using Core.Entities.AppMessages;
using Core.Entities.Compensation;
using Core.Entities.Configuration;
using Core.Entities.FreeSeats;
using Core.Entities.MotUpdates;
using Core.Entities.RailUpdates;
using Core.Entities.Translation;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Entities.Push;
using API.Dtos.Surveys;
using Core.Entities.Surveys;
using API.Dtos.Forms;
using Core.Entities.Forms;
using Core.Entities.Stations;
using Core.Entities;
using API.Dtos.URLTranslations;

namespace API.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Station, StationResponseDto>()
            .ForMember(dest => dest.StationName, src => src.MapFrom<StationNameResolver>())
            .ForMember(dest => dest.Location, src => src.MapFrom(p => new StationResponseDto.StationLocation { Latitude = p.Latitude, Lontitude = p.Lontitude }))
            .ForMember(dest => dest.Synonyms, src =>
                src.MapFrom((s, t, u, ctx) => s.Synonym.Where(q => q.LanguageId == (int)ctx.Items["languageId"]).Select(q => q.SynonymName)));

        CreateMap<Station, StationAllDto>();

        CreateMap<UpdateStationNode, RailUpdateResponseDto>()
            .ForMember(dest => dest.Image, src => src.MapFrom(p => p.ReportImage))
            .AfterMap((x, y, ctx) => FetchLanguage(x, y, ctx));

        CreateMap<FreeSeatsRequestDto, FreeSeatsRequest>()
            .ForMember(dest => dest.TrainDate, src => src.MapFrom(p => p.Date.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.TrainNumbers, src => src.MapFrom(p => p.TrainNumbers.Split(",", System.StringSplitOptions.TrimEntries)));
        CreateMap<FreeSeats.TrainFreeSeat, FreeSeatsResponseDto>()
            .ForMember(dest => dest.FreeSeats, src => src.MapFrom(p => p.SeatsAvailable));

        CreateMap<PushRegistrationRequestDto, PushRegistration>();
        CreateMap<PushRegistration, PushRegistrationResponseDto>()
            .ForMember(dest => dest.PushRegistrationId, src => src.MapFrom(p => p.Id));
        CreateMap<PushNotificationsByDateDto, PushRouting>()
            .ForMember(dest => dest.PushNotificationsByDate, src => src.MapFrom(p => p.PushNotifications)) ;
        CreateMap<PushNotificationInfoDto, PushNotificationsByDate>();
       
        CreateMap<PushNotificationsByWeekDayDto, PushRouting>()
            .ForMember(dest => dest.PushNotificationsByWeekDay, src => src.MapFrom(p => p.PushNotifications));
        CreateMap<PushNotificationInfoByWeekDto, PushNotificationsByWeekDay>();

        CreateMap<SurveysData, SurveysDataDto>();
        CreateMap<SurveysDataDto, SurveysData>();
        CreateMap<IEnumerable<SurveysDataDto>, IEnumerable<SurveysData>>();

        CreateMap<SurveysResults, SurveysResultsDto>();
        CreateMap<SurveysResultsDto, SurveysResults>();
        CreateMap< IEnumerable<SurveysResultsDto>, IEnumerable<SurveysResults>>();

        CreateMap<FullForms, FullFormsDto>();
        CreateMap<FullFormsDto, FullForms>();

        CreateMap<StationImageDto, StationImage>();
        CreateMap<StationImage, StationImageDto>();
        CreateMap<StationImageComplete, StationImageCompleteDto>();
        CreateMap<StationImageCompleteDto, StationImageComplete>();

        CreateMap<PushNotificationRequestQueryDto, PushNotificationQuery>();

        CreateMap<PushNotificationUpdateRequestDto, PushNotificationsByWeekDay>()
            .ForMember(dest => dest.PushRoutingId, src => src.MapFrom(p => p.PushRoutingId))
            .ForMember(dest => dest.day1, src => src.MapFrom(p => p.WeekDays.day1))
            .ForMember(dest => dest.day2, src => src.MapFrom(p => p.WeekDays.day2))
            .ForMember(dest => dest.day3, src => src.MapFrom(p => p.WeekDays.day3))
            .ForMember(dest => dest.day4, src => src.MapFrom(p => p.WeekDays.day4))
            .ForMember(dest => dest.day5, src => src.MapFrom(p => p.WeekDays.day5))
            .ForMember(dest => dest.day6, src => src.MapFrom(p => p.WeekDays.day6))
            .ForMember(dest => dest.day7, src => src.MapFrom(p => p.WeekDays.day7))
            ;
      
        CreateMap<PushNotification, TrainRouteInfo>();

        CreateMap<MotUpdateRequestDto, MotUpdateRequest>();
               CreateMap<MailingListPutDto, MailingList>();
        CreateMap<MailingList, MailingListPutDto>();
       
        CreateMap<CompensationRequestDto, Compensation>()
            .ForMember(dest => dest.OriginStationId, src => src.MapFrom(p => p.OriginCode))
            .ForMember(dest => dest.DestinationStationId, src => src.MapFrom(p => p.DestinationCode));
        CreateMap<Compensation, CompensationResponseDto>()
            .ForMember(dest => dest.RowId, src => src.MapFrom(p => p.Id))
            .ForMember(dest => dest.ResponseStatus, src => src.MapFrom(p => p.Id > 0));

        CreateMap<CompensationSearchRequestDto, SearchCompensation>();
        CreateMap<Compensation, CompensationSearchResult>()
            .ForMember(dest => dest.CardRecievedDate, src => src.MapFrom(p => p.CardrecievedDate))
            .ForMember(dest => dest.DestinationCode, src => src.MapFrom(p => p.DestinationStationId))
            .ForMember(dest => dest.OriginCode, src => src.MapFrom(p => p.OriginStationId));

        CreateMap<IEnumerable<Translation>, TranslationResponseDto>()
            .AfterMap((x, y) => SetTranslation(x, y));

        CreateMap<TranslationRequestDto, TranslationRequest>();

        CreateMap<IEnumerable<URLTranslation>, URLTranslationResponseDto>()
          .AfterMap((x, y) => SetURLTranslation(x, y));

        CreateMap<URLTranslationRequestDto, URLTranslationRequest>();

        CreateMap<IEnumerable<Translation>, TranslationByLanguageResponseDto>()
            .ForMember(dest => dest.Translations, src => src.MapFrom<TranslationsResolver>());

        CreateMap<PopupMessagesRequestDto, MessageRequest>();
        CreateMap<PopUpMessages, PopupMessagesResponseDto>()
            .ForMember(dest => dest.MessageBody, src => src.MapFrom<PopupMessageLanguageResolver>())
            .ForMember(dest => dest.Title, src => src.MapFrom<PopupTitleLanguageResolver>());
        
        CreateMap<PopupMessagesStationRequestDto, MessageRequest>();
        CreateMap<PopUpMessages, PopupMessagesWithStationsResponseDto>()
            .ForMember(dest => dest.MessageBody, src => src.MapFrom<PopupMessageStationLanguageResolver>())
            .ForMember(dest => dest.Title, src => src.MapFrom<PopupTitleStationLanguageResolver>());

        CreateMap<Configuration, ConfigurationResponseDto>();

        CreateMap<CompensationOtpRequestDto, CompensationOtpRequest>();
        CreateMap<CompensationOtpSearchRequestDto, CompensationOtpRequest>();

        CreateMap<PushNotificationsByWeekDay, PushByWeekDayDto>();
        CreateMap<PushNotificationsByDate, PushByDateDto>();

    }

    private static void SetTranslation(IEnumerable<Translation> translations, TranslationResponseDto to)
    {
        foreach (Translation translation in translations)
        {
            to.He.Add(new KeyValuePair<string, string>(translation.Key, translation.Hebrew));
            to.En.Add(new KeyValuePair<string, string>(translation.Key, translation.English));
            to.Ar.Add(new KeyValuePair<string, string>(translation.Key, translation.Arabic));
            to.Ru.Add(new KeyValuePair<string, string>(translation.Key, translation.Russian));
        }
    }

    private static void SetURLTranslation(IEnumerable<URLTranslation> translations, URLTranslationResponseDto to)
    {
        foreach (URLTranslation translation in translations)
        {
            to.He.Add(new KeyValuePair<string, string>(translation.Key, translation.Hebrew));
            to.En.Add(new KeyValuePair<string, string>(translation.Key, translation.English));
            to.Ar.Add(new KeyValuePair<string, string>(translation.Key, translation.Arabic));
            to.Ru.Add(new KeyValuePair<string, string>(translation.Key, translation.Russian));
        }
    }


    private static void FetchLanguage(UpdateStationNode from, RailUpdateResponseDto to, ResolutionContext ctx)
    {
        string languageId = ctx.Items["languageId"].ToString();

        switch (Enum.Parse(typeof(Languages), languageId))
        {
            case Languages.Hebrew:
                to.Update(from.NameHeb, from.UpdateContentHeb, from.UpdateLinkHeb);
                break;
            case Languages.English:
                to.Update(from.NameEng, from.UpdateContentEng, from.UpdateLinkEng);
                break;
            case Languages.Arabic:
                to.Update(from.NameArb, from.UpdateContentArb, from.UpdateLinkArb);
                break;
            case Languages.Russian:
                to.Update(from.NameRus, from.UpdateContentRus, from.UpdateLinkRus);
                break;
        }
    }
}
