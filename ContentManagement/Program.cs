//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Hosting;
//using Serilog;
//using ContentManagement.Extensions;

//namespace ContentManagement;

//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//               .ConfigureAppConfiguration((hostingContext, config) => {
//                   var environment = hostingContext.HostingEnvironment;
//                   var settings = config.Build();
//                   var keyVaultEndpoint = settings["AzureKeyVaultEndpoint"];
//                   config.AddAzureConfigurationServices(environment);
//               })
//                .UseSerilog((hostingContext, loggerConfig) =>
//                {
//                    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
//                })
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                });
//    }


using ContentManagement.Extension;
using ContentManagement.Extensions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var environment = builder.Environment;

builder.Configuration.AddAzureConfigurationServices(environment);

builder.Services.AddApiVersioning(config =>
{
    // Specify the default API Version as 1.0
    config.DefaultApiVersion = new ApiVersion(1, 0);
    // If the client hasn't specified the API version in the request, use the default API version number 
    config.AssumeDefaultVersionWhenUnspecified = true;
    // Advertise the API versions supported for the particular endpoint
    config.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
    // note: the specified format code will format the version as "'v'major[.minor][-status]"  
    options.GroupNameFormat = "'v'VVV";

    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat  
    // can also be used to control the format of the API version in route templates  
    options.SubstituteApiVersionInUrl = true;
});

builder.Host.UseSerilog((hostingContext, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.InstanceName = string.Empty;
    opt.Configuration = builder.Configuration.GetConnectionString("azureCache");
});

builder.Services.AddCors(setup =>
{
    setup.AddPolicy("cors_policy", policy =>
    {
        _ = policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<RailDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));

builder.Services.AddSwaggerService();

builder.Services.ConfigureApplicationService(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}

app.UseSwaggerService();

app.UseHttpsRedirection();

app.UseCors("cors_policy");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();