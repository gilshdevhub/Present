using Core.Errors;
using Core.Helpers;
using System.Net;
using System.Text.Json;

namespace SmsSending.Middlewares;

public class ResponseWrapperMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ResponseWrapperMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        var currentBody = context.Response.Body;

        using (var memoryStream = new MemoryStream())
        {
            //set the current response to the memorystream.
            context.Response.Body = memoryStream;

            await _next(context);

            //reset the body 
            context.Response.Body = currentBody;
            memoryStream.Seek(0, SeekOrigin.Begin);

            string readToEnd = await new StreamReader(memoryStream).ReadToEndAsync().ConfigureAwait(false);
            string version = _configuration.GetValue<string>("Version");

            CommonApiResponse response = null;
            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                var objResult = JsonSerializer.Deserialize<object>(readToEnd);
                response = CommonApiResponse.Create((HttpStatusCode)context.Response.StatusCode, version, 1, objResult, null);
            }
            else
            {
                ApiErrorResponse apiErrorResponse = JsonSerializer.Deserialize<ApiErrorResponse>(readToEnd, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                response = CommonApiResponse.Create((HttpStatusCode)context.Response.StatusCode, version, 0, null, apiErrorResponse);
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }
    }
}

public static class ResponseWrapperMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseWrapper(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ResponseWrapperMiddleware>();
    }
}