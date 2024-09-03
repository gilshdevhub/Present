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

public class MotUpdatesService : IMotUpdatesService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientService _httpClient;
    private readonly RailDbContext _context;
    private readonly SpecialDbContext _specialContext;
   
    public MotUpdatesService(IConfiguration configuration, IHttpClientService httpClient, RailDbContext context,
          /*  ICacheService cacheService,*/ SpecialDbContext specialDbContext)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _context = context;
               _specialContext = specialDbContext;
    }

    public async Task<MotUpdateResponse> GetMotUpdatesAsync(MotUpdateRequest motUpdateRequest)
    {
        string motUrl = string.Format(_configuration.GetSection("mot").Value, motUpdateRequest.MonitoringRef, motUpdateRequest.LineRef);

        string json = await _httpClient.GetRailInfoAsync(motUrl, "application/json").ConfigureAwait(false);
        MotUpdateResponse motUpdate = JsonSerializer.Deserialize<MotUpdateResponse>(json);
        return motUpdate;
    }

    public async Task<object> GetMotUpdateListsAsync()
    {
        try
        {
            var motConvertionList = (await GetMotConvertionListContentAsync().ConfigureAwait(false)).ToArray();
            var motTransportList = (await GetPublicTransportAsync(string.Empty).ConfigureAwait(false)).ToArray();
            if (motConvertionList.Length == 0 || motTransportList.Length == 0)
            {
                return null;
            }
            var res = new
            {
                ConvertionList = motConvertionList,
                TransportList = motTransportList
            };
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<ResponseMotUpdatesByTrainStationDto> GetMotUpdatesByTrainStationsAsync(int stationId, DateTime startTime)
    {

        var motStation = (await GetMotConvertionListContentAsync().ConfigureAwait(false)).Where(x => x.StationId == stationId).ToArray();
        if (motStation.Length == 0 || startTime < DateTime.Now.AddMinutes(-31))
        {
            return null;
        }
        string json = string.Empty;
        MotUpdateRequest motUpdateRequest = new();
        var _busStops = String.Join(",", (motStation.Select(x => x.MonitoringRef)).ToArray());
        var _busStopsToDb = String.Join(",", (motStation.Select(x => x.MonitoringRef)).ToArray());
        motUpdateRequest.MonitoringRef = _busStops;

        string motUrl = string.Format(_configuration.GetSection("mot").Value.Replace("&LineRef={1}", "&StartTime={1}"), motUpdateRequest.MonitoringRef, String.Format("{0:yyyyMMddTHHmmssPff}", startTime));
        try
        {
            json = await _httpClient.GetRailInfoAsync(motUrl, "application/json").ConfigureAwait(false);
            IEnumerable<PublicTransportStations> t;
            MotUpdateResponse motUpdate = JsonSerializer.Deserialize<MotUpdateResponse>(json);
            var p = motUpdate.Siri.ServiceDelivery.StopMonitoringDelivery.ToList().SelectMany(x => x.MonitoredStopVisit).Select(x => x.MonitoredVehicleJourney.DestinationRef).ToArray().Distinct();
            _busStopsToDb += "," + String.Join(",", p);
            try
            {
                t = await GetPublicTransportAsync(_busStopsToDb).ConfigureAwait(false);
                ResponseMotUpdatesByTrainStationDto res = new ResponseMotUpdatesByTrainStationDto
                {
                    MotResponse = motUpdate,
                    MotStations = t
                };
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    public async Task<BLSDto> GetSiriAsync(int stationId)
    {

        MotConvertion[] motStation = (await GetMotConvertionListContentAsync().ConfigureAwait(false)).Where(x => x.StationId == stationId).ToArray();
        if(motStation.Length == 0)
        {
            return null;
        }
        MotUpdateRequest motUpdateRequest = new();
        var _busStops = String.Join(",", (motStation.Select(x => x.MonitoringRef)).ToArray());
        var lineRef = "";
        motUpdateRequest.MonitoringRef = _busStops;

        string motUrl = string.Format(_configuration.GetSection("mot").Value, motUpdateRequest.MonitoringRef);
        string BLS2URL = string.Format(_configuration.GetSection("BLS2URL").Value, motUrl);
        try
        {
            string json = await _httpClient.GetRailInfoAsync(BLS2URL, "application/json").ConfigureAwait(false);
            MotUpdateResponse motUpdate = await GetSiri( motUrl).ConfigureAwait(false);
            var p = motUpdate.Siri.ServiceDelivery.StopMonitoringDelivery.ToList().SelectMany(x => x.MonitoredStopVisit).Select(x => x.MonitoredVehicleJourney.LineRef).ToArray().Distinct();
            lineRef = String.Join(",", p);
            try
            {
                IEnumerable<BusTripHeadSignsDto> t = await GetBusTripHeadSignsAsync(lineRef).ConfigureAwait(false);
                BLSDto res = new BLSDto
                {
                    MotResponse = motUpdate,
                    BusTripHeadSigns = t,
                    NearBusStops = motStation.Select(x => new NearBusStopsDto
                    {
                        busStopLocation = x.BusStopLocation,
                        monitoringRef = x.MonitoringRef,
                    }).ToArray(),
                };
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<MotUpdateResponse> GetSiri(string motUrl)
    {
        string json = await _httpClient.GetRailInfoAsync(motUrl, "application/json").ConfigureAwait(false);

        MotUpdateResponse motUpdate = JsonSerializer.Deserialize<MotUpdateResponse>(json);
        return motUpdate;
    }

    public async Task<IEnumerable<MotConvertion>> GetMotConvertionListContentAsync()
    {
                                                   IEnumerable<MotConvertion>  allConvertionList = await _context.MotConvertion.ToArrayAsync().ConfigureAwait(false);
       
                                   
               return allConvertionList;

    }
    public async Task<IEnumerable<PublicTransportStations>> GetPublicTransportAsync(string stations)
    {
        try
        {
            var BS = new SqlParameter { ParameterName = "@busStops", Value = stations };

            IEnumerable<PublicTransportStations> dto = await _specialContext.PublicTransportStations
         .FromSqlRaw("exec [dbo].[sp_GetPublicTransport] @busStops;", BS)
        .ToArrayAsync().ConfigureAwait(false);
            return dto;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<IEnumerable<BusTripHeadSignsDto>> GetBusTripHeadSignsAsync(string lineRefs)
    {
        try
        {
            var BS = new SqlParameter { ParameterName = "@lineRefParam", Value = lineRefs };

            var dto = await _specialContext.BusTripHeadSigns
         .FromSqlRaw("exec [dbo].[GetBusTripHeadSigns] @lineRefParam;", BS).AsNoTracking()
        .ToArrayAsync().ConfigureAwait(false);
            return dto;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<GTFSBLSDto> GetGTFSAsync(int stationId)
    {
        MotConvertion[] motStation = (await GetMotConvertionListContentAsync().ConfigureAwait(false)).Where(x => x.StationId == stationId).ToArray();

        string json = string.Empty;
        MotUpdateRequest motUpdateRequest = new();
        var _busStops = String.Join(",", (motStation.Select(x => x.MonitoringRef)).ToArray());

        try
        {
            var BS = new SqlParameter { ParameterName = "@busStops", Value = _busStops };

            IEnumerable<BusStopTimesDto> dto = await _specialContext.BusStopTimes
                .FromSqlRaw("exec [dbo].[GetRelevantStopTimes] @busStops;", BS)
                .ToArrayAsync().ConfigureAwait(false);


            GTFSBLSDto gTFSBLSDto = new GTFSBLSDto
            {
                BusStopTimes = dto,
                NearBusStops = motStation.Select(x => new NearBusStopsDto
                {
                    busStopLocation = x.BusStopLocation,
                    monitoringRef = x.MonitoringRef,
                }).ToArray()
            };
            return gTFSBLSDto;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}