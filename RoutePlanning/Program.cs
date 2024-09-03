using Asp.Versioning;
using Core.Middlewares;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RoutePlanning.Extensions;
using RoutePlanning.Helpers;
using RoutePlanning.Middlewares;
//using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureConfigurationServices(builder.Environment);
builder.Services.AddControllers();

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

//builder.Host.UseSerilog((hostingContext, loggerConfig) =>
//{
//    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
//});

builder.Services.AddDbContext<RailDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));
builder.Services.AddSwaggerService();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureApplicationService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfiles));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerService();
app.UseCors("cors_policy");
app.MapControllers();
app.UseHttpsRedirection();
app.UseInvalidApiRequestBlocker();
app.UseResponseWrapper();
app.UseApiExceptionError();
app.UseRouting();
app.UseAuthorization();

app.Run();