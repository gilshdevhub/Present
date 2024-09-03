using API.Dtos;
using API.Extensions;
using API.Helpers;
using API.Middlewares;
using Asp.Versioning;
using Aspose.Svg.Services;
using Core.Config;
using Core.Middlewares;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;

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

builder.Services.AddDbContext<RailDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));
builder.Services.AddDbContext<SpecialDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));
builder.Services.AddSwaggerService();
builder.Services.ConfigureApplicationService(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfiles));
#region JWT Rsa Autentication
JwtRsaConfig jwtRsaConfig = new JwtRsaConfig();
builder.Configuration.GetSection(string.Format(JwtRsaConfig.JwtRsaConfiguration, "Compensations")).Bind(jwtRsaConfig);
builder.Services.ConfigureAutenticationService(jwtRsaConfig);
#endregion

//builder.Services.Configure<ValuesOptions>(builder.Configuration.GetSection("ValuesOptions"));

//builder.WebHost.UseSerilog((Serilog.ILogger)builder.Configuration.GetRequiredSection("Serilog"));
builder.Host.UseSerilog((hostingContext, loggerConfig) =>
                {
                    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
                });


#region Firebase Configuration
FirebaseCredentialConfig firebaseConfig = builder.Configuration.GetSection(FirebaseCredentialConfig.Firebase).Get<FirebaseCredentialConfig>();
string json = JsonConvert.SerializeObject(firebaseConfig);
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(json)
});
#endregion

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
app.Run();