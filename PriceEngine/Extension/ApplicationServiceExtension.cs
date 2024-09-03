using Core.Config;
using Core.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace PriceEngine.Extension
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DapperContext>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IPriceEngineService, PriceEngineService>();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext => CheckModelState(actionContext);
            });

            services.AddCors(action =>
            {
                action.AddPolicy("cors_policy", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            _ = services.Configure<CacheConfig>(configuration.GetSection(CacheConfig.CacheSettings));


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
