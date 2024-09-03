
using Core.Config;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace PisData.Extensions;

public static class ApplicationServiceExtension
{
    public static void ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<IPisData, PisDataService>();
        _ = services.AddHttpClient<IHttpClientService, HttpClientService>();
        _ = services.AddScoped<IAzureKeyVaultService, AzureKeyVaultService>();
        _ = services.AddScoped<IVersioningService, VersioningService>();
        _ = services.AddScoped<ICacheService, RedisCacheService>();
        _ = services.AddScoped<IConfigurationService, ConfigurationService>();
        _ = services.AddScoped<WriteToLogFilterAttribute>();
        _ = services.Configure<CacheConfig>(configuration.GetSection(CacheConfig.CacheSettings));
        _ = services.Configure<MailConfig>(configuration.GetSection(MailConfig.MailSettings));
        _ = services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = actionContext => CheckModelState(actionContext);
        });

        _ = services.Configure<APIMailSettingsConfig>(configuration.GetSection(APIMailSettingsConfig.APIMailSettings));
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
