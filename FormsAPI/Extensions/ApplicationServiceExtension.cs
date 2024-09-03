
using Core.Config;
using Core.Errors;
using Core.Filters;
using Core.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace FormsAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<WriteToLogFilterAttribute>();
            services.AddTransient<IAppFormsService, AppFormsService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IJwtHandlerService, JwtHandlerService>();

            services.AddScoped<WriteToLogFilterAttribute>();
            services.AddHttpClient<IHttpClientService, HttpClientService>();

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
}
