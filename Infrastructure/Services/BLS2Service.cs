using Core.Entities.MotUpdates;
using Core.Entities.Push;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Interfaces;
using Dapper;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.Json;

namespace Infrastructure.Services;

public class BLS2Service : IBLS2Service
{
    private readonly IHttpClientService _httpClient;

    public BLS2Service(IHttpClientService httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MotUpdateResponse> GetSiri(string motUrl)
    {
        string json = await _httpClient.GetRailInfoAsync(motUrl, "application/json").ConfigureAwait(false);

        MotUpdateResponse motUpdate = JsonSerializer.Deserialize<MotUpdateResponse>(json);
        return motUpdate;
    }

}