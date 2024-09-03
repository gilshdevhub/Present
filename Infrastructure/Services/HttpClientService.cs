using Core.Interfaces;
using System.Text;

namespace Infrastructure.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Accept.Clear();
    }

    public async Task<string> GetRailInfoAsync(string apiUri, string mediaType)
    {
        string data;

        _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType));

        using (HttpResponseMessage response = await _httpClient.GetAsync(new Uri(apiUri)).ConfigureAwait(false))
        {
            _ = response.EnsureSuccessStatusCode();
            data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        return data;
    }

         
   
               
          public async Task<string> PostRailInfoAsync(string body, string apiUri, string mediaType)
    {
        string data;

        using (StringContent content = new(content: body, encoding: Encoding.UTF8))
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType));
            using HttpResponseMessage response = await _httpClient.PostAsync(apiUri, content).ConfigureAwait(false);
            _ = response.EnsureSuccessStatusCode();
            data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        return data;
    }

    public async Task<string> GetBodyInfoAsync(string apiUri, object body, string mediaType)
    {
        string data;

        var newRequest = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(apiUri),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body), Encoding.UTF8, mediaType)
        };
        using (HttpResponseMessage response = await _httpClient.SendAsync(newRequest).ConfigureAwait(false))
        {
            _ = response.EnsureSuccessStatusCode();
            data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        return data;
    }

         
                     
      }
