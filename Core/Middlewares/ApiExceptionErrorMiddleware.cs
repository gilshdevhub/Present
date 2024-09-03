using Core.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Core.Middlewares;

public class ApiExceptionErrorMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ApiExceptionErrorMiddleware> _logger;

    public ApiExceptionErrorMiddleware(RequestDelegate next, ILogger<ApiExceptionErrorMiddleware> logger)//, string mailSubject)//, IMailService mailService
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ApiExceptionResponse(ex.Message, ex.StackTrace);
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true });

            await context.Response.WriteAsync(json);
        }
    }
}

public static class ApiExceptionErrorMiddlewareExtensions
{
    public static IApplicationBuilder UseApiExceptionError(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiExceptionErrorMiddleware>();
    }
}
