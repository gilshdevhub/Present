using Core.Entities.FreeSeats;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class FreeSeatsService : IFreeSeatsService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<FreeSeatsService> _logger;
    private readonly IMailService _mailService;

    public FreeSeatsService(IConfiguration configuration, IHttpClientService httpClient, ILogger<FreeSeatsService> logger, IMailService mailService)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;
        _mailService = mailService;
    }

    public async Task<FreeSeats> GetFreeSeatsAsync(FreeSeatsRequest request)
    {
        FreeSeatsHeaderRequest freeSeatsHeaders = new();

        foreach (int trainNumber in request.TrainNumbers)
        {
            freeSeatsHeaders.FreeSeats.Add(new FreeSeatsHeader
            {
                TrainNumber = trainNumber,
                TrainDate = request.TrainDate,
                DestStation = request.DestinationStation,
                FromStation = request.OriginStation
            });
        }

        string json = JsonConvert.SerializeObject(freeSeatsHeaders);
        string requestUri = _configuration.GetSection("RailSeatsUrl").Value;

        try
        {
            string result = await _httpClient.PostRailInfoAsync(json, requestUri, "application/json").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<FreeSeats>(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "free seats failer");
                       return null;
        }
    }
}
