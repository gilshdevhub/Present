using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Core.Middlewares;

public class InvalidApiRequestBlockerMiddleware
{
    private readonly RequestDelegate _next;

    public InvalidApiRequestBlockerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path != default && context.Request.Path.Value.Contains("api", System.StringComparison.CurrentCultureIgnoreCase))
        {
            await _next(context);
        }
    }
}

public static class InvalidApiRequestBlockerMiddlewareExtensions
{
    public static IApplicationBuilder UseInvalidApiRequestBlocker(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<InvalidApiRequestBlockerMiddleware>();
    }
}
