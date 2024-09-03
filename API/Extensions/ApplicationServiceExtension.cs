using Azure.Storage.Blobs;
using Core.Config;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Infrastructure.Services.AccurecyIndexService;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ApplicationServiceExtension
{
    public static void ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped(_ => { return new BlobServiceClient(configuration.GetSection("TendersSettings:AzureBlobStorage").Value); });
        services.AddScoped<IMessengerService, MessengerService>();
        services.AddScoped<IPaymantsService, PaymantsService>();
        services.AddScoped<IKirtusConnetctionService, KirtusConnetctionService>();
        services.AddScoped<IRailUpdatesService, RailUpdatesService>();
        services.AddScoped<IStationsService, StationsService>();
        services.AddScoped<IFreeSeatsService, FreeSeatsService>();
        services.AddScoped<IMotUpdatesService, MotUpdatesService>();
        services.AddScoped<ICompensationService, CompensationService>();
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<ITranslationService, TranslationService>();
        services.AddScoped<IURLTranslationService, URLTranslationService>();
        services.AddScoped<IVersioningService, VersioningService>();
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IPopUpMessagesService, PopUpMessagesService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
        services.AddScoped<IPushNotificationsService, PushNotificationsService>();
        services.AddScoped<IAccurecyIndexService, AccurecyIndexService>();
        services.AddScoped<ISurveysService, SurveysService>();
        services.AddScoped<IFaresService, FaresService>();
        services.AddScoped<IStationImage, StationImageService>();
        services.AddTransient<IAppFormsService, AppFormsService>();
        services.AddScoped<IManagmentSystemObjects, ManagmentSystemObjectsService>();
        services.AddScoped<ITendersService, TendersService>();
        services.AddScoped<ITenderTypesService, TenderTypesService>();
        services.AddScoped<IMailingListService, MailingListService>();
        services.AddScoped<IMeetingsService, MeetingsService>();
        services.AddScoped<ISingleSupplierIService, SingleSupplierIService>();
        services.AddScoped<IExemptionNotices, ExemptionNoticesService>();
        services.AddScoped<IPlanningAndRatesService, PlanningAndRatesService>();
        services.AddScoped<ITendersCommonService, TendersCommonService>();
        services.AddScoped<IEligibilityService, EligibilityService>();
        services.AddScoped<IClosedStationsService, ClosedStationsService>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IMyTripService, MyTripService>();
        services.AddScoped<IBackUpService, BackUpService>();
        services.AddScoped<IBackgroundImageService, BackgroundImageService>();

               services.AddScoped<DapperContext>();
        services.AddScoped<WriteToLogFilterAttribute>();

        services.AddTransient<IMailService, MailService>();
        services.AddTransient<IJwtHandlerService, JwtHandlerService>();
        services.AddTransient<IFileStorageUplaodService, FileStorageUplaodService>();

        services.AddHttpClient<IHttpClientService, HttpClientService>();

        services.Configure<PushwooshConfig>(configuration.GetSection(PushwooshConfig.Pushwoosh));
        services.Configure<RailUpdatesConfig>(configuration.GetSection(RailUpdatesConfig.RailUpdates));
        services.Configure<TelemassageConfig>(configuration.GetSection(TelemassageConfig.Telemessage));
        services.Configure<MailConfig>(configuration.GetSection(MailConfig.MailSettings));
        services.Configure<CacheConfig>(configuration.GetSection(CacheConfig.CacheSettings));
        services.Configure<JwtRsaConfig>(configuration.GetSection(string.Format(JwtRsaConfig.JwtRsaConfiguration, "Compensations")));

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext => CheckModelState(actionContext);
        });

        services.AddCors(action =>
        {
            action.AddPolicy("cors_policy", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
    }

    private static IActionResult CheckModelState(ActionContext context)
    {
        IEnumerable<string> errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value.Errors).Select(e => e.ErrorMessage).ToArray();

        var errorResponse = new ApiValidationResponse { Errors = errors };
        return new BadRequestObjectResult(errorResponse);
    }
}
