using Microsoft.AspNetCore.Mvc;
using SiteManagement.Extension;
using Core.Middlewares;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using SiteManagement.Helpers;
using Aspose.Svg.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using SiteManagement.Handlers;
using Core.Config;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Identity;
using Core.Entities.Identity;
using Serilog;
using SiteManagement.Middlewares;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureConfigurationServices(builder.Environment);
builder.Services.AddControllers();

#region Api Versioning
builder.Services.AddApiVersioning(config =>
{
       config.DefaultApiVersion = new ApiVersion(1, 0);
       config.AssumeDefaultVersionWhenUnspecified = true;
       config.ReportApiVersions = true;
}).AddApiExplorer (options =>
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

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));
builder.Services.AddDbContext<SpecialDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));
builder.Services.AddDbContext<RailDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));
builder.Services.AddSwaggerService();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureIdentityServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(setup =>
{
    setup.AddPolicy("cors_policy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

#region Firebase Configuration
FirebaseCredentialConfig firebaseConfig = builder.Configuration.GetSection(FirebaseCredentialConfig.Firebase).Get<FirebaseCredentialConfig>();
string json = JsonConvert.SerializeObject(firebaseConfig);
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(json)
});
#endregion

#region Authorization Policy
builder.Services.AddAuthorization(options => options.AddPolicy("PageRole", policy => policy.Requirements.Add(new PageRoleRequirement())));
builder.Services.AddTransient<IAuthorizationHandler, PageRoleRequirementHandler>();
#endregion
builder.Host.UseSerilog((hostingContext, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}

app.UseSwaggerService();

app.UseHttpsRedirection();

app.UseCors("cors_policy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseWrapper();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
}); 

using (IServiceScope scope = app.Services.CreateScope())
{
    System.IServiceProvider services = scope.ServiceProvider;
    ILoggerFactory loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
        await SeedIdentityContext.SeedRolesAsync(userManager, roleManager).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occured during migration!");
    }
}
app.Run();