using Azure.Storage.Blobs;
using Core.Config;
using Core.Interfaces;
using Core.Interfaces.Identity;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.Identity;

namespace SiteManagement.Extension;

public static class AppServiceExtension
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<DapperContext>();
        _ = services.AddScoped(_ => { return new BlobServiceClient(configuration.GetSection("TendersSettings:AzureBlobStorage").Value); });
       
        _ = services.AddScoped<ITokenService, TokenService>();
        _ = services.AddScoped<IConfigurationService, ConfigurationService>();
        _ = services.AddScoped<IStationsService, StationsService>();
        _ = services.AddScoped<IStationGateService, StationGateService>();
        _ = services.AddScoped<ITranslationService, TranslationService>();
        _ = services.AddScoped<IURLTranslationService, URLTranslationService>();
        _ = services.AddScoped<IPopUpMessagesService, PopUpMessagesService>();
        _ = services.AddScoped<ITrainWarningsService, TrainWarningsService>();
        _ = services.AddTransient<IAppFormsService, AppFormsService>();
        _ = services.AddScoped<ICacheService, RedisCacheService>();
        _ = services.AddScoped<IMessengerService, MessengerService>();
        _ = services.AddScoped<IStationImage, StationImageService>();
        _ = services.AddHttpClient<IHttpClientService, HttpClientService>();
        _ = services.AddScoped<IManagmentSystemObjects, ManagmentSystemObjectsService>();
        _ = services.AddScoped<IManagmentLogger, ManagmentLoggerService>();
        _ = services.Configure<PushwooshConfig>(configuration.GetSection(PushwooshConfig.Pushwoosh));
        _ = services.Configure<TelemassageConfig>(configuration.GetSection(TelemassageConfig.Telemessage));
        _ = services.Configure<CacheConfig>(configuration.GetSection(CacheConfig.CacheSettings));
        _ = services.AddScoped<IPushNotificationsService, PushNotificationsService>();
        _ = services.AddScoped<IContentPagesService, ContentPagesService>();
        _ = services.AddScoped<IEligibilityService, EligibilityService>();
        _ = services.AddScoped<ITendersCommonService, TendersCommonService>();
        _ = services.AddTransient<IMailService, MailService>();
        _ = services.AddScoped<IRoleService, RoleService>();
        _ = services.AddScoped<IPagesService, PagesService>();
        _ = services.AddScoped<ISpecialPushNotificationsService, SpecialPushNotificationsService>();
        _ = services.AddScoped<ISurveysService, SurveysService>();
        _ = services.AddScoped<IStationActivityHoursTemplatesService, StationActivityHoursTemplatesService>();
        _ = services.AddScoped<IStationGateActivityHoursService, StationGateActivityHoursService>();
        _ = services.AddScoped<ITendersService, TendersService>();
        _ = services.AddScoped<IPlanningAndRatesService, PlanningAndRatesService>();
        _ = services.AddScoped<ITenderTypesService, TenderTypesService>();
        _ = services.AddScoped<IMailingListService, MailingListService>();
        _ = services.AddScoped<IMeetingsService, MeetingsService>();
        _ = services.AddScoped<ISingleSupplierIService, SingleSupplierIService>();
        _ = services.AddScoped<IExemptionNotices, ExemptionNoticesService>();
        _ = services.AddScoped<IFileStorageUplaodService, FileStorageUplaodService>();
        _ = services.AddScoped<IClosedStationsService, ClosedStationsService>();
        _ = services.AddScoped<IPriceEngineService, PriceEngineService>();
        _ = services.AddScoped<IBackgroundImageService, BackgroundImageService>();
        _ = services.AddScoped<IRailUpdatesService, RailUpdatesService>();

    }
}
