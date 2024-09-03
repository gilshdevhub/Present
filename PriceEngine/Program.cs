using Asp.Versioning;
using Core.Middlewares;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using PriceEngine.Extension;
using PriceEngine.Middlewares;

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
builder.Services.AddSwaggerService();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureApplicationService(builder.Configuration);
builder.Services.AddDbContext<RailDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("rail")));

builder.Services.AddStackExchangeRedisCache(opt =>
{
    opt.InstanceName = string.Empty;
    opt.Configuration = builder.Configuration.GetConnectionString("azureCache");
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwaggerService();



app.UseCors("cors_policy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseInvalidApiRequestBlocker();
app.UseResponseWrapper();
app.Run(); 
//