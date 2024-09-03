using Core.Errors;
using Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Core.Filters;

public class WriteToLogFilterAttribute : ActionFilterAttribute
{
    private readonly ILogger<WriteToLogFilterAttribute> _logger;
    private readonly IConfiguration _configuration;

    private Guid _guid;

    public WriteToLogFilterAttribute(ILogger<WriteToLogFilterAttribute> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _guid = Guid.NewGuid();
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        StringBuilder sb = new("Http Request Information:");
        _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "Request Id:", new string('\t', 2), _guid);
        _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "Method:", new string('\t', 3), context.HttpContext.Request.Method);
        _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "Host:", new string('\t', 3), context.HttpContext.Request.Host);
        _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "Path:", new string('\t', 3), context.HttpContext.Request.Path);
        _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "QueryString:", new string('\t', 1), context.HttpContext.Request.QueryString);
        _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "Request Body:", new string('\t', 1), await ReadBodyContextAsync(context.HttpContext.Request).ConfigureAwait(false));
        _logger.LogInformation(sb.ToString());

        var resultContext = await next().ConfigureAwait(false);

        sb = new("Http Response Information:");
        _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "Response Id:", new string('\t', 1), _guid);
        _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "Method:", new string('\t', 3), context.HttpContext.Request.Method);

        if (resultContext.Result is ObjectResult objectResult)
        {
            string version = _configuration.GetSection("Version").Value;

            CommonApiResponse commonApiResponse;
            if (objectResult.StatusCode == (int)HttpStatusCode.OK)
            {
                commonApiResponse = CommonApiResponse.Create((HttpStatusCode)objectResult.StatusCode, version, 1, objectResult.Value, null);
            }
            else
            {
                commonApiResponse = CommonApiResponse.Create((HttpStatusCode)objectResult.StatusCode, version, 0, null, (ApiErrorResponse)objectResult.Value);
            }

            string response = JsonSerializer.Serialize(commonApiResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            _ = sb.AppendFormat("{0}{1}{2}{3}{4}", Environment.NewLine, new string('\t', 5), "Response Body:", new string('\t', 1), response);
            _logger.LogInformation(sb.ToString());
        }
    }

    private static async Task<string> ReadBodyContextAsync(HttpRequest request)
    {
        string body = string.Empty;

        request.EnableBuffering();
        _ = request.Body.Seek(0, SeekOrigin.Begin);

        using (var reader = new StreamReader(request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        return body;
    }
}
