﻿
using Core.Config;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BLS2.Extensions;

public static class ApplicationServiceExtension
{
    public static void ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<IBLS2Service, BLS2Service>();
        _ = services.AddHttpClient<IHttpClientService, HttpClientService>();
        _ = services.AddScoped<IAzureKeyVaultService, AzureKeyVaultService>();
        _ = services.AddScoped<IVersioningService, VersioningService>();

        _ = services.AddScoped<WriteToLogFilterAttribute>();

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
