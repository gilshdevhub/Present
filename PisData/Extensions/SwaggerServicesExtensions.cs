﻿using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace PisData.Extensions;

public static class SwaggerServicesExtensions
{
    public static void AddSwaggerService(this IServiceCollection services)
    {
        _ = services.AddSwaggerGen(c =>
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
        _ = app.UseSwagger(c => { c.RouteTemplate = "/swagger/{documentName}/swagger.json"; });

        _ = app.UseSwaggerUI(c =>
        {
            var service = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (ApiVersionDescription description in service.ApiVersionDescriptions)
            {
                if (!description.IsDeprecated)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            }
        });
    }

    private static OpenApiInfo CreateMetaInfoAPIVersion(ApiVersionDescription apiDescription)
    {
        return new OpenApiInfo
        {
            Title = "PIS",
            Version = apiDescription.ApiVersion.ToString(),
            Description = "PIS Data"
        };
    }
}
