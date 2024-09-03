using Core.Entities.Fares;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class FaresService : IFaresService
{
    private readonly RailDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessengerService> _logger;


    public FaresService(ILogger<MessengerService> logger, RailDbContext context, IConfiguration configuration)
    {
        _context = context;

        _configuration = configuration;
        _logger = logger;
    }

    public async Task<CodesDataDto> GetCodesData()
    {
        CodesDataDto result = new();
        result.ettCodes = await _context.EttCodes.ToArrayAsync();
        result.profileCodes = await _context.ProfileCodes.ToArrayAsync();
        return result;
    }

    public async Task<FaresData> GetFares(FaresRequestData requestData)
    {
        int version = _context.FaresVersions.Select(p => p.Version).FirstOrDefault();
        var result = await GetTariffTrip(version, requestData.Profile_Code, requestData.ETT_Code, requestData.Station_Origin_Code, requestData.Station_Destination_Code);
        return result;
    }

    private async Task<FaresData> GetTariffTrip(int VersionId, int Profile_code, int ETT_Code, int Station_Origin_Code, int Station_Destination_Code)
    {
        string UserName = _configuration.GetSection("Fares").GetSection("UserName").Value;
        string SystemOrigin = _configuration.GetSection("Fares").GetSection("SystemOrigin").Value;
        string PWD = _configuration.GetSection("Fares").GetSection("PWD").Value;
        string url = string.Format(_configuration.GetSection("Fares").GetSection("GetTariffTrip").Value, VersionId, SystemOrigin, UserName, PWD, Station_Origin_Code, Station_Destination_Code, Profile_code, ETT_Code);

        HttpClient httpClient = new();
        FaresData info = new();
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(new Uri(url)).ConfigureAwait(false);
            _ = response.EnsureSuccessStatusCode();
            string data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            info = JsonConvert.DeserializeObject<FaresData>(data);
        }
        catch (Exception ex)
        {
            _logger.LogError("error occured ", ex);
        }

        return info;
    }
}
