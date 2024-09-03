using Microsoft.IO;

namespace API.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }

    public async Task Invoke(HttpContext context)
    {
        Guid requestId = Guid.NewGuid();
        await LogRequest(context, requestId);
        await LogResponse(context, requestId);
    }

    private async Task LogRequest(HttpContext context, Guid requestId)
    {
        context.Request.EnableBuffering();

        await using var requestStream = _recyclableMemoryStreamManager.GetStream();
        await context.Request.Body.CopyToAsync(requestStream);

        System.Text.StringBuilder sb = new System.Text.StringBuilder("Http Request Information:");
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Request Id: {requestId}"));
        sb.Append(string.Format("{0}{1}{2}",  $"{Environment.NewLine}", new string('\t', 5), $"Schema: {context.Request.Scheme}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Method: {context.Request.Method}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Host: {context.Request.Host}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Path: {context.Request.Path}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"QueryString: {context.Request.QueryString}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"LocalIpAddress: {context.Connection.LocalIpAddress}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"RemoteIpAddress: {context.Connection.RemoteIpAddress}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Request Body: {ReadStreamInChunks(requestStream)}"));
        _logger.LogInformation(sb.ToString());

        context.Request.Body.Position = 0;
    }
    private async Task LogResponse(HttpContext context, Guid requestId)
    {
        var originalBodyStream = context.Response.Body;

        await using var responseBody = _recyclableMemoryStreamManager.GetStream();
        context.Response.Body = responseBody;
        
        await _next(context);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        System.Text.StringBuilder sb = new System.Text.StringBuilder("Http Response Information:");
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Response Id: {requestId}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Schema: {context.Request.Scheme}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Host: {context.Request.Host}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Path: {context.Request.Path}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"QueryString: {context.Request.QueryString}"));
        sb.Append(string.Format("{0}{1}{2}", $"{Environment.NewLine}", new string('\t', 5), $"Response Body: {text}"));
        _logger.LogInformation(sb.ToString());

        await responseBody.CopyToAsync(originalBodyStream);
    }
    private static string ReadStreamInChunks(Stream stream)
    {
        const int readChunkBufferLength = 4096;

        stream.Seek(0, SeekOrigin.Begin);
        
        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);
        
        var readChunk = new char[readChunkBufferLength];
        int readChunkLength;
        do
        {
            readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
            textWriter.Write(readChunk, 0, readChunkLength);
        }
        while (readChunkLength > 0);
        
        return textWriter.ToString();
    }
}