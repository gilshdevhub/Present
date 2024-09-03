using Core.Config;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.RoutePlanning;
using Microsoft.AspNetCore.Mvc;

namespace RoutePlanning.Extensions;

public static class ApplicationServiceExtension
{
    public static void ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITimeTableService, TimeTableService>();
        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
        services.AddScoped<IStationsService, StationsService>();
        services.AddScoped<IManagmentSystemObjects, ManagmentSystemObjectsService>();
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<IMessengerService, MessengerService>();
        services.AddScoped<IFreeSeatsService, FreeSeatsService>();
        services.AddScoped<IRailUpdatesService, RailUpdatesService>();
        services.AddScoped<WriteToLogFilterAttribute>();

        services.AddTransient<IMailService, MailService>();

        services.AddHttpClient<IHttpClientService, HttpClientService>();

        services.Configure<MailConfig>(configuration.GetSection(MailConfig.MailSettings));
        services.Configure<CacheConfig>(configuration.GetSection(CacheConfig.CacheSettings));
        services.Configure<LuzWSConfig>(configuration.GetSection(LuzWSConfig.LuzSettings));

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
