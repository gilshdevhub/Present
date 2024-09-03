using Core.Middlewares;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using PisData.Extensions;
using PisData.Helpers;
using PisData.Middlewares;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureConfigurationServices(builder.Environment);
builder.Services.AddControllers();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


#region Api Versioning
builder.Services.AddApiVersioning(config =>
{
       config.DefaultApiVersion = new ApiVersion(1, 0);
       config.AssumeDefaultVersionWhenUnspecified = true;
       config.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
          options.GroupNameFormat = "'v'VVV";

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

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<RailDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));
builder.Services.AddSwaggerService();
builder.Services.ConfigureApplicationService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfiles));
//builder.Host.UseSerilog((hostingContext, loggerConfig) =>
//{
//    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
//});

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
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseResponseWrapper();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});
app.Run();
