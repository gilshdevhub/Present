using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


namespace FormsAPI.Extensions
{
    public static class SwaggerServicesExtensions
    {
        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider();
                var service = provider.GetRequiredService<IApiVersionDescriptionProvider>();
               
                foreach (ApiVersionDescription description in service.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, CreateMetaInfoAPIVersion(description));
                }
                
            });
        }

        public static void UseSwaggerService(this IApplicationBuilder app)
        {
            app.UseSwagger(c => { c.RouteTemplate = "/swagger/{documentName}/swagger.json"; });

            app.UseSwaggerUI(c =>
            {
                var service = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (ApiVersionDescription description in service.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
        }

        private static OpenApiInfo CreateMetaInfoAPIVersion(ApiVersionDescription apiDescription)
        {
            return new OpenApiInfo
            {
                Title = "Forms",
                Version = apiDescription.ApiVersion.ToString(),
                Description = "Sending forms to customer service"
            };
        }
    }
}
