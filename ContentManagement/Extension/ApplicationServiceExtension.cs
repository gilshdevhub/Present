using Core.Config;
using Core.Interfaces;
using Infrastructure.Services;

namespace ContentManagement.Extensions;
public static class ApplicationServiceExtension
{
    public static void ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<IUmbracoService, UmbracoService>();
        _ = services.AddScoped<IAzureKeyVaultService, AzureKeyVaultService>();
        _ = services.AddScoped<ICacheService, RedisCacheService>();
        _ = services.AddScoped<ITendersService, TendersService>();
        _ = services.AddScoped<ISingleSupplierIService, SingleSupplierIService>();
        _ = services.AddScoped<IExemptionNotices, ExemptionNoticesService>();
        _ = services.AddScoped<IPlanningAndRatesService, PlanningAndRatesService>();
        _ = services.AddScoped<ITendersCommonService, TendersCommonService>();
        _ = services.AddHttpClient<IHttpClientService, HttpClientService>();
        _ = services.AddScoped<IMeetingsService, MeetingsService>();
        _ = services.Configure<CacheConfig>(configuration.GetSection(CacheConfig.CacheSettings));
    }
}

