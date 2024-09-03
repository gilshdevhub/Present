using Core.Entities;
using Core.Entities.MyTravel;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Core.Entities.Vouchers.MyTrip;

namespace Infrastructure.Services;

public class MyTripService : IMyTripService
{
    private readonly RailDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly SpecialDbContext _specialDbContext;
    public MyTripService(RailDbContext context, ICacheService cacheService, SpecialDbContext specialDbContext)

    {
        _context = context;
        _cacheService = cacheService;
        _specialDbContext = specialDbContext;
    }



    public async Task<IEnumerable<VisaTrainMainData>> GetClosestSations(ClosedTrainsRequestDto request)
    {
               List<List<int>> trainsForFiltering = new List<List<int>>();
        var MJdistance = Double.Parse(_context.ConfigurationParameter.Where(x => x.Key == "MJCloseDistance").Select(x => x.ValueMob).FirstOrDefault());

        var MJtime = Double.Parse(_context.ConfigurationParameter.Where(x => x.Key == "MJMaxMonitoringTime").Select(x => x.ValueMob).FirstOrDefault());
        try
        {
            IEnumerable<TrainLocation> TrainsLocationData = await _cacheService.GetAsync<IEnumerable<TrainLocation>>(CacheKeys.TrainsLocationData).ConfigureAwait(false);
            if (TrainsLocationData == null)
            {
                TrainsLocationData = await _context.TrainLocation.ToArrayAsync().ConfigureAwait(false);
                await _cacheService.SetAsync(CacheKeys.TrainsLocationData, TrainsLocationData).ConfigureAwait(false);
            }

        }
        catch (Exception ex)
        {

        }

        foreach (var item in request.Data)
        {

            List<TrainsTempResult> trainsTempResult = new List<TrainsTempResult>();
            double lat = Decimal.ToDouble(item.UserLatitude);
            double lon = Decimal.ToDouble(item.UserLongitude);
                       foreach (var train in _context.TrainLocation)
            {
                double stationLatitude = Decimal.ToDouble(train.Latitude);
                double stationLontitude = Decimal.ToDouble(train.Longitude);
                var tempDistatnce = distance(lat, stationLatitude,
                              lon, stationLontitude);
                var df = Math.Abs((train.TravelDate - item.SamplingTime).TotalSeconds);
                if (tempDistatnce <= MJdistance && Math.Abs((train.TravelDate - item.SamplingTime).TotalSeconds) <= MJtime
                    && (trainsTempResult.Count == 0 || tempDistatnce <= trainsTempResult.Select(x => x.Distanse).FirstOrDefault())
                    )
                {
                    trainsTempResult.Add(new TrainsTempResult { TrainNum = train.TrainNr, Distanse = tempDistatnce });
                }

            }


            if (trainsTempResult.Any())
            {
                trainsForFiltering.Add(trainsTempResult.Select(x => x.TrainNum).ToList());
            }
        }

        List<int> tempResult = new List<int>();


        if (!tempResult.Any())
        {
            trainsForFiltering.ForEach(x =>
            {
                tempResult.AddRange(x);
            });
            tempResult = (tempResult
              .GroupBy(s => s)
              .Select(g => new ClosedTrainsWithOrder { TrainNum = g.Key }).OrderByDescending(x => x.Count).ToArray()
              ).Select(x => x.TrainNum).ToList();
        }

        IEnumerable<VisaTrainMainData> result = null;

        IEnumerable<VisaTrainMainData> visaTrainMainData = await _cacheService.GetAsync<IEnumerable<VisaTrainMainData>>(CacheKeys.VisaData);

        var f = tempResult.ToList();
        if (tempResult.Count > 0)
        {
            result = visaTrainMainData.Where(x => tempResult.Contains(x.NMBRAK)).ToList();
            return result;
        }
        else
        {
            return new List<VisaTrainMainData>();
        }

    }

    static double toRadians(
           double angleIn10thofaDegree)
    {
                      return (angleIn10thofaDegree *
                       Math.PI) / 180;
    }
    static double distance(double lat1,
                           double lat2,
                           double lon1,
                           double lon2)
    {

                                    lon1 = toRadians(lon1);
        lon2 = toRadians(lon2);
        lat1 = toRadians(lat1);
        lat2 = toRadians(lat2);

               double dlon = lon2 - lon1;
        double dlat = lat2 - lat1;
        double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                   Math.Cos(lat1) * Math.Cos(lat2) *
                   Math.Pow(Math.Sin(dlon / 2), 2);

        double c = 2 * Math.Asin(Math.Sqrt(a));

                             double r = 6371;

               return (c * r);
    }

   

}
