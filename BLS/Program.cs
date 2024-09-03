using Asp.Versioning;
using BLS.Extensions;
using BLS.Helpers;
using BLS.Middlewares;
using Core.Config;
using Core.Middlewares;
using FirebaseAdmin;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureConfigurationServices(builder.Environment);
string[] allowsOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string>().Split(',');
string[] allowsMethods = builder.Configuration.GetSection("AllowsMethods").Get<string>().Split(',');

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder =>
        {
            builder.WithOrigins(allowsOrigins)
           .WithMethods(allowsMethods)
           .AllowAnyHeader();
        });
});
builder.Services.AddControllers();

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

#region BLS Versioning
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

//#region Azure Cache
//builder.Services.AddStackExchangeRedisCache(async opt =>
//{
//    string configuration = builder.Configuration.GetConnectionString("azureCache");
//    if (string.IsNullOrEmpty(configuration))
//    {
//        throw new InvalidOperationException("Redis connection string is not configured.");
//    }

//    var configurationOptions = ConfigurationOptions.Parse(configuration);
//    string managedIdentityClientId = builder.Configuration.GetSection("managedIdentityClientId").Value;
//    if (string.IsNullOrEmpty(managedIdentityClientId))
//    {
//        throw new InvalidOperationException("Managed Identity Client ID is not configured.");
//    }

//    // Configure for Azure with User Assigned Managed Identity
//    await configurationOptions.ConfigureForAzureWithUserAssignedManagedIdentityAsync(managedIdentityClientId);

//    opt.ConfigurationOptions = configurationOptions;
//});
//#endregion

builder.Services.AddDbContext<RailDbContext>(options =>
{
    string railConnectionString = builder.Configuration.GetConnectionString("rail");
    if (string.IsNullOrEmpty(railConnectionString))
    {
        throw new InvalidOperationException("Rail database connection string is not configured.");
    }
    options.UseSqlServer(railConnectionString);
});

builder.Services.AddDbContext<SpecialDbContext>(options =>
{
    string specialConnectionString = builder.Configuration.GetConnectionString("rail");
    if (string.IsNullOrEmpty(specialConnectionString))
    {
        throw new InvalidOperationException("Special database connection string is not configured.");
    }
    options.UseSqlServer(specialConnectionString);
});

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
   app.UseCors("AllowOrigins");
app.UseInvalidApiRequestBlocker();
app.UseResponseWrapper();
app.Run();