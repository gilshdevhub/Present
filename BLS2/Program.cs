using Asp.Versioning;
using BLS2.Extensions;
using BLS2.Middlewares;
using Core.Middlewares;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddSwaggerService();
builder.Services.ConfigureApplicationService(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}

app.UseApiExceptionError();
app.UseSwaggerService();
app.MapControllers();
app.UseHttpsRedirection();
app.UseRouting();
app.UseInvalidApiRequestBlocker();
app.UseResponseWrapper();
app.Run();