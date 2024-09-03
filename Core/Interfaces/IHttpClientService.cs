namespace Core.Interfaces;

public interface IHttpClientService
{
    Task<string> GetRailInfoAsync(string apiUri, string mediaType);
    Task<string> PostRailInfoAsync(string body, string apiUri, string mediaType);
    Task<string> GetBodyInfoAsync(string apiUri, object body, string mediaType);
}
