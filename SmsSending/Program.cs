using Asp.Versioning;
using Core.Middlewares;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SmsSending.Extensions;
using SmsSending.Helpers;
using SmsSending.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureConfigurationServices(builder.Environment);
builder.Services.AddControllers();

#region Api Versioning
builder.Services.AddApiVersioning(config =>
{
    // Specify the default API Version as 1.0
    config.DefaultApiVersion = new ApiVersion(1, 0);
    // If the client hasn't specified the API version in the request, use the default API version number 
    config.AssumeDefaultVersionWhenUnspecified = true;
    // Advertise the API versions supported for the particular endpoint
    config.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
    // note: the specified format code will format the version as "'v'major[.minor][-status]"  
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat  
    // can also be used to control the format of the API version in route templates  
    options.SubstituteApiVersionInUrl = true;
});
#endregion
#region Azure Cache
builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.InstanceName = string.Empty;
    opt.Configuration = builder.Configuration.GetConnectionString("azureCache");
});
#endregion

builder.Host.UseSerilog((hostingContext, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services.AddDbContext<RailDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));
builder.Services.AddSwaggerService();
builder.Services.ConfigureApplicationService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}
app.UseApiExceptionError();
app.UseSwaggerService();
app.UseCors("cors_policy");
app.MapControllers();
app.UseHttpsRedirection();
app.UseRouting();
app.UseInvalidApiRequestBlocker();
app.UseResponseWrapper();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();