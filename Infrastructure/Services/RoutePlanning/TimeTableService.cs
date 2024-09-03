using Core.Config;
using Core.Entities.AppMessages;
using Core.Entities.Configuration;
using Core.Entities.FreeSeats;
using Core.Entities.Notifications;
using Core.Entities.TimeTable;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Interfaces;
using Core.Interfaces.TimeTable;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.ServiceModel;
using System.Xml;

namespace Infrastructure.Services.RoutePlanning;

public class TimeTableService : ITimeTableService
{
    private readonly ITimeTableServiceChannel _proxy;
    private readonly LuzWSConfig _luzConfig;
    private readonly IConfigurationService _configurationService;
    private readonly INotificationsService _notificationsService;
    private readonly IStationsService _stationsService;
    private readonly RailDbContext _railDbContext;
    private readonly ILogger<TimeTableService> _logger;
    private readonly IMailService _mailService;
    private readonly IFreeSeatsService _freeSeatsService;

    private enum ClientMessageId
    {
        DefaultResults = 1,
        FirstDateResults = 2,
        NoResults = 3,
        DateOfSearchAndNextDateResults = 4,
        TodayResults = 5,
        RjpaCommFail = 6,
        TrainDateIsInThePast = 50,
        TrainNumberNotExist = 51
    }

    public TimeTableService(IOptions<LuzWSConfig> luzConfig, IConfigurationService configurationService, INotificationsService notificationsService,
        IStationsService stationsService, RailDbContext railDbContext, ILogger<TimeTableService> logger, IMailService mailService, IFreeSeatsService freeSeatsService)
    {
        _luzConfig = luzConfig.Value;
        _proxy = GetProxy<ITimeTableServiceChannel>();
        _configurationService = configurationService;
        _notificationsService = notificationsService;
        _stationsService = stationsService;
        _railDbContext = railDbContext;
        _logger = logger;
        _mailService = mailService;
        _freeSeatsService = freeSeatsService;
    }

    public async Task<TrainTimeTableRespnse> GetTrainTimeTableAsync(TrainTimeTableRequest request)
    {
        List<Travel> travels = new();
        CultureInfo cultureInfo = new("he-IL");

        var allConfigurations = await GetConfigurationBySysType(request.SystemType);
        int numOfResultsToShow = int.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "NumOfResultsToShow", StringComparison.OrdinalIgnoreCase)).Value);
               int searchNumberOfDays = int.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "SearchNumberOfDays", StringComparison.OrdinalIgnoreCase)).Value);
               int minutesBeforeSearch = int.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "MinutesBeforeSearch", StringComparison.OrdinalIgnoreCase)).Value);
               bool voucherActive = bool.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "VoucherActive", StringComparison.OrdinalIgnoreCase)).Value);
       
        bool continueToSearch = false;
        int counter = 0;

        TrainTimeTableRespnse trainTimeTableRespnse = new() { NumOfResultsToShow = numOfResultsToShow, Travels = travels };
        DateTime dateTimeToSearch = DateTime.Parse(request.Date, cultureInfo).AddTicks(request.Hours.Ticks).AddMinutes(0 - minutesBeforeSearch);

        try
        {

            do
            {
                DateTime requestDate = DateTime.Parse(request.Date, cultureInfo).AddDays(counter);
                string luzRequestDate = requestDate.ToString("dd/MM/yyyy");

                IEnumerable<Travel> tmpTravels = await GetTravelsAsync(request, luzRequestDate).ConfigureAwait(false);

                if (tmpTravels.Any())
                {
                    travels.AddRange(tmpTravels);
                    if (voucherActive)
                    {
                        try
                        {
                            await SetTravelFreeSeatsAsync(travels, request, requestDate).ConfigureAwait(false);
                        }
                        catch (FreeSeatsCommException)
                        {
                            trainTimeTableRespnse.FreeSeatsError = true;
                        }
                    }
                }

                int totalGoodResults = request.ScheduleType == ScheduleTypes.ByDeparture ?
                    travels.Count(p => p.DepartureTime >= dateTimeToSearch) : travels.Count(p => p.ArrivalTime >= dateTimeToSearch);

                counter++;
                continueToSearch = (counter < searchNumberOfDays) && (totalGoodResults == 0);
            }
            while (continueToSearch);

            if (travels.Count > 0)
            {
                if (request.ScheduleType == ScheduleTypes.ByDeparture)
                    trainTimeTableRespnse.Travels = travels.OrderBy(p => p.DepartureTime).ToArray();
                if (request.ScheduleType == ScheduleTypes.ByArrival)
                    trainTimeTableRespnse.Travels = travels.OrderBy(p => p.ArrivalTime).ToArray();
                await SetIndexesAsync(trainTimeTableRespnse, request, dateTimeToSearch).ConfigureAwait(false);
                await SetTravelMessagesAsync(trainTimeTableRespnse, request).ConfigureAwait(false);
            }

            SetClientMessageId(trainTimeTableRespnse, request, dateTimeToSearch);
        }
        catch (RjpaCommFailException)
        {
            trainTimeTableRespnse.ClientMessageId = (int)ClientMessageId.RjpaCommFail;
        }

        return trainTimeTableRespnse;
    }
    public async Task<TrainTimeTableRespnse> GetTrainTimeTableBeforeDateAsync(TrainTimeTableRequest request)
    {
        TrainTimeTableRespnse trainTimeTableRespnse = await SearchBeforeAfterAsync(request, isBefore: true).ConfigureAwait(false);
        return trainTimeTableRespnse;
    }
    public async Task<TrainTimeTableRespnse> GetTrainTimeTableAfterDateAsync(TrainTimeTableRequest request)
    {
        TrainTimeTableRespnse trainTimeTableRespnse = await SearchBeforeAfterAsync(request, isBefore: false).ConfigureAwait(false);
        return trainTimeTableRespnse;
    }
    public async Task<TrainTimeTableRespnse> GetTrainTimeTableByTrainNumberAsync(TrainTimeTableByTrainNumberRequest request)
    {
        List<Travel> travels = new();
        CultureInfo cultureInfo = new("he-IL");

        var allConfigurations = await GetConfigurationBySysType(request.SystemType);
        int numOfResultsToShow = int.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "NumOfResultsToShow", StringComparison.OrdinalIgnoreCase)).Value);
               bool voucherActive = bool.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "VoucherActive", StringComparison.OrdinalIgnoreCase)).Value);
       
        TrainTimeTableRespnse trainTimeTableRespnse = new() { NumOfResultsToShow = numOfResultsToShow, Travels = travels };
        DateTime dateTimeToSearch = DateTime.Parse(request.Date, cultureInfo);

        if (DateTime.Compare(dateTimeToSearch.Date, DateTime.Now.Date) < 0)
        {
            trainTimeTableRespnse.ClientMessageId = (int)ClientMessageId.TrainDateIsInThePast;
            return trainTimeTableRespnse;
        }

        string luzRequestDate = dateTimeToSearch.ToString("dd/MM/yyyy");
        travels.AddRange(await GetTravelsAsync(request, luzRequestDate).ConfigureAwait(false));
        trainTimeTableRespnse.Travels = travels.OrderBy(p => p.DepartureTime).ToArray();

        int trainIndex = trainTimeTableRespnse.Travels.ToList().FindIndex(travel => travel.Trains.Any(p => p.TrainNumber == request.TrainNUmber));
        if (trainIndex == -1)
        {
            trainTimeTableRespnse.ClientMessageId = (int)ClientMessageId.TrainNumberNotExist;
            trainTimeTableRespnse.Travels = null;
        }
        else
        {
            trainTimeTableRespnse.StartFromIndex = trainTimeTableRespnse.OnFocusIndex = trainIndex;
            await SetTravelMessagesAsync(trainTimeTableRespnse, request).ConfigureAwait(false);
            if (voucherActive)
            {
                await SetTravelFreeSeatsAsync(travels, request, dateTimeToSearch).ConfigureAwait(false);
            }
        }

        return trainTimeTableRespnse;
    }

    private async Task<TrainTimeTableRespnse> SearchBeforeAfterAsync(TrainTimeTableRequest request, bool isBefore)
    {
        List<Travel> travels = new();
        CultureInfo cultureInfo = new("he-IL");
        var allConfigurations = await GetConfigurationBySysType(request.SystemType);
        int numOfResultsToShow = int.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "NumOfResultsToShow", StringComparison.OrdinalIgnoreCase)).Value);
               int searchNumberOfDays = int.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "SearchNumberOfDays", StringComparison.OrdinalIgnoreCase)).Value);
               bool voucherActive = bool.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "VoucherActive", StringComparison.OrdinalIgnoreCase)).Value);
       
        TrainTimeTableRespnse trainTimeTableRespnse = new() { NumOfResultsToShow = numOfResultsToShow, Travels = travels };

        int counter = 1;
        string luzRequestDate;
        bool continueToSearch = false;

        try
        {
            do
            {
                DateTime requestDate = DateTime.Parse(request.Date, cultureInfo).AddDays(isBefore ? 0 - counter : counter);
                luzRequestDate = requestDate.ToString("dd/MM/yyyy");

                IEnumerable<Travel> tmpTravels = await GetTravelsAsync(request, luzRequestDate).ConfigureAwait(false);

                if (tmpTravels.Any())
                {
                    travels.AddRange(tmpTravels);
                    if (voucherActive && !trainTimeTableRespnse.FreeSeatsError)
                    {
                        try
                        {
                            await SetTravelFreeSeatsAsync(travels, request, requestDate).ConfigureAwait(false);
                        }
                        catch (FreeSeatsCommException)
                        {
                            trainTimeTableRespnse.FreeSeatsError = true;
                        }
                    }
                }

                counter++;
                continueToSearch = (counter < searchNumberOfDays) && (travels.Count == 0);
            }
            while (continueToSearch);

            trainTimeTableRespnse.Travels = travels.OrderBy(p => p.DepartureTime).ToArray();
            trainTimeTableRespnse.ClientMessageId = trainTimeTableRespnse.Travels.Any() ? (int)ClientMessageId.DefaultResults : (int)ClientMessageId.NoResults;

            if (trainTimeTableRespnse.Travels.Any())
            {
                request.Date = luzRequestDate;
                await SetTravelMessagesAsync(trainTimeTableRespnse, request).ConfigureAwait(false);
            }
            if (isBefore)
            {
                trainTimeTableRespnse.StartFromIndex = trainTimeTableRespnse.Travels.Count() - 1;
            }
        }
        catch (RjpaCommFailException)
        {
            trainTimeTableRespnse.ClientMessageId = (int)ClientMessageId.RjpaCommFail;
        }

        return trainTimeTableRespnse;
    }
    private async Task<List<Travel>> GetTravelsAsync(TrainTimeTableRequest request, string requestDate)
    {
        List<Travel> travels = new();

        try
        {
            XmlNode luz = await _proxy.GetLuzAsync(request.SysytemId, request.SystemUserName, request.SystemPass, 
                request.FromStation, request.ToStation, requestDate, 0)
                .ConfigureAwait(false);

            XmlNode tsd = luz.SelectSingleNode("/TSD");
            XmlNode tps = luz.SelectSingleNode("/TPS");

            XmlNodeList directTrains = luz.SelectNodes("/Directs/Direct");
            foreach (XmlNode travel in directTrains)
            {
                travels.Add(new Travel(travel, tsd, tps));
            }

            XmlNodeList inDirectTrains = luz.SelectNodes("/Indirects/Indirect");
            foreach (XmlNode travel in inDirectTrains)
            {
                travels.Add(new Travel(travel, tsd, tps));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTravelsAsync failed");
                              }

        return travels;
    }
    private T GetProxy<T>()
    {
        T proxy;

        BasicHttpBinding binding = new()
        {
            SendTimeout = TimeSpan.FromSeconds(_luzConfig.Timeout),
            MaxBufferSize = int.MaxValue,
            MaxReceivedMessageSize = int.MaxValue,
            AllowCookies = true,
            ReaderQuotas = XmlDictionaryReaderQuotas.Max
        };

        binding.Security.Mode = BasicHttpSecurityMode.None;
        EndpointAddress address = new(_luzConfig.Url);
        ChannelFactory<ITimeTableServiceChannel> factory = new(binding, address);
        proxy = (T)factory.CreateChannel();
        return proxy;
    }
    private async Task SetIndexesAsync(TrainTimeTableRespnse response, TrainTimeTableRequest request, DateTime dateTimeToSearch)
    {
        var allConfigurations = await GetConfigurationBySysType(request.SystemType);
        int displayBeforeResult = int.Parse(allConfigurations.SingleOrDefault(p => string.Equals(p.Key, "DisplayBeforeResult", StringComparison.OrdinalIgnoreCase)).Value);
               int firstMatchIndex = GetFirstMatchIndex(response.Travels.ToArray(), dateTimeToSearch, request.ScheduleType);

               int numberOfResults = request.ScheduleType == ScheduleTypes.ByDeparture ?
            response.Travels.Count(p => p.DepartureTime.ToShortDateString() == dateTimeToSearch.ToShortDateString() && p.DepartureTime >= dateTimeToSearch) :
            response.Travels.Count(p => p.ArrivalTime.ToShortDateString() == dateTimeToSearch.ToShortDateString() && p.ArrivalTime >= dateTimeToSearch);

        if (firstMatchIndex == 0)
        {
            response.StartFromIndex = 0;
            response.OnFocusIndex = 0;
        }
        else if (numberOfResults == 0)
        {
            response.StartFromIndex = firstMatchIndex - 1;
            if (request.SystemType == SystemTypes.Web)
            {
                response.OnFocusIndex = firstMatchIndex - 1;
            }
        }
        else
        {
            response.StartFromIndex = firstMatchIndex - displayBeforeResult;
            if (request.SystemType == SystemTypes.Web)
            {
                response.OnFocusIndex = firstMatchIndex;
            }
        }
    }
    private static int GetFirstMatchIndex(Travel[] travels, DateTime dateTimeToSearch, ScheduleTypes scheduleType)
    {
        int matchIndex = 0;

        bool found = false;

        for (int indx = 0; indx < travels.Length && !found; indx++)
        {
            matchIndex = indx;

            if (scheduleType == ScheduleTypes.ByDeparture)
            {
                found = travels[indx].DepartureTime >= dateTimeToSearch;
            }
            else
            {
                found = travels[indx].ArrivalTime >= dateTimeToSearch;
            }
        }

        return matchIndex;
    }
    private async Task<string> GetConfigurationItemAsync(string key, SystemTypes systemType)
    {
        IEnumerable<Configuration> configurations = await _configurationService.GetConfigurationsBySystemTypeAsync(systemType).ConfigureAwait(false);

        string value = configurations.Where(p => string.Equals(p.Key, key, StringComparison.OrdinalIgnoreCase))
            .Select(p => p.Value).SingleOrDefault();
        return value;
    }
    private async Task<IEnumerable<Configuration>> GetConfigurationBySysType(SystemTypes systemTypes)
    {
        IEnumerable<Configuration> configurations = await _configurationService.GetConfigurationsBySystemTypeAsync(systemTypes).ConfigureAwait(false);
        return configurations;
    }
    private static void SetClientMessageId(TrainTimeTableRespnse response, TrainTimeTableRequest request, DateTime dateTimeToSearch)
    {
        if (!response.Travels.Any())
        {
                       response.ClientMessageId = (int)ClientMessageId.NoResults;
        }
        else
        {
            int totalGoodResults = request.ScheduleType == ScheduleTypes.ByDeparture ?
                response.Travels.Count(p => p.DepartureTime.ToShortDateString() == dateTimeToSearch.ToShortDateString() && p.DepartureTime >= dateTimeToSearch) :
                response.Travels.Count(p => p.ArrivalTime.ToShortDateString() == dateTimeToSearch.ToShortDateString() && p.ArrivalTime >= dateTimeToSearch);

            if (totalGoodResults == 0)
            {
                var dates = response.Travels.GroupBy(p => p.DepartureTime.ToShortDateString()).Select(p => new { date = p.Key });
                IEnumerable<string> datesToCheck = dates.Select(p => p.date).ToArray();

                if (!datesToCheck.Contains(dateTimeToSearch.ToShortDateString()))
                {
                                       response.ClientMessageId = (int)ClientMessageId.FirstDateResults;
                }
                else
                {
                    if (datesToCheck.Count() == 2)
                    {
                                               response.ClientMessageId = (int)ClientMessageId.DateOfSearchAndNextDateResults;
                    }
                    else
                    {
                                               response.ClientMessageId = (int)ClientMessageId.TodayResults;
                    }
                }
            }
        }
    }
    private async Task SetTravelMessagesAsync(TrainTimeTableRespnse trainTimeTableRespnse, TrainTimeTableRequest request)
    {
        try
        {
            DateTime dateTimeToSearch = DateTime.Parse(request.Date, new CultureInfo("he-IL")).AddTicks(request.Hours.Ticks);

            IEnumerable<int> trainNumbers = trainTimeTableRespnse.Travels.SelectMany(p => p.Trains.Select(t => t.TrainNumber)).ToArray();
            string trains = string.Empty;
            trains = trainNumbers.Aggregate(trains, (x, y) => $"{y},{x}");
            trains = trains[0..^1];

            IEnumerable<TrainWarning> trainWarnings = await GetTrainWarningsAsync(dateTimeToSearch, trains).ConfigureAwait(false);
            IEnumerable<AutomationNotification> automationNotifications = await GetAutomationNotificationsAsync(dateTimeToSearch, trains).ConfigureAwait(false);

            if (!automationNotifications.Any() && !trainWarnings.Any())
            {
                return;
            }

            foreach (TrainWarning trainWarning in trainWarnings)
            {
                IEnumerable<Travel> travels = trainTimeTableRespnse.Travels.Where(p => p.Trains.Any(t => t.TrainNumber == trainWarning.TrainNumber)).ToArray();
                foreach (Travel travel in travels)
                {
                    travel.TravelMessages.Add(new()
                    {
                        TrainNumber = trainWarning.TrainNumber,
                        Message = GetMessageBody(trainWarning, request.LanguageId),
                        Title = GetMessageTitle(trainWarning, request.LanguageId),
                        Sevirity = trainWarning.WarningTypeId
                    });
                }
            }

            if (automationNotifications.Any())
            {
                IEnumerable<Station> stations = await _stationsService.GetStationsAsync().ConfigureAwait(false);
                IEnumerable<NotificationType> notificationTypes = await _notificationsService.GetNotificationTypesAsync().ConfigureAwait(false);
                IEnumerable<ConfigurationParameter> configurations = await _configurationService.GetAllItemsAsync().ConfigureAwait(false);

                trainNumbers = automationNotifications.Select(p => p.TrainNumber).ToArray();
                trains = string.Empty;
                trains = trainNumbers.Aggregate(trains, (x, y) => $"{y},{x}");
                trains = trains[0..^1];
                IEnumerable<RailSchedual> railSchedules = await GetRailSchedualAsync(dateTimeToSearch, trains).ConfigureAwait(false);

                if (railSchedules.Any())
                {
                    foreach (AutomationNotification automationNotification in automationNotifications)
                    {
                        try
                        {
                            Travel travel = trainTimeTableRespnse.Travels
                                .SingleOrDefault(t => t.Trains.Any(p => p.TrainNumber == automationNotification.TrainNumber && p.DepartureTime.Date == automationNotification.TrainDate.Date));
                            if (travel == null)
                            {
                                continue;
                            }

                            Train train = travel.Trains.Single(t => t.TrainNumber == automationNotification.TrainNumber);

                            int toStationOrder;
                            int fromStationOrder;
                            int changedStationOrder;
                            bool hasFound = false;

                            IEnumerable<RailSchedual> tempRailScheduals = railSchedules.Where(r => r.TrainNumber == train.TrainNumber).ToArray();
                            IEnumerable<int> trainStations = tempRailScheduals.Select(p => p.StationId).ToArray();

                            if (trainStations.Contains(request.FromStation) && trainStations.Contains(request.ToStation) && trainStations.Contains((int)automationNotification.ChangedStationId))
                            {
                                                               fromStationOrder = tempRailScheduals.Single(p => p.StationId == request.FromStation).StationOrder;
                                toStationOrder = tempRailScheduals.Single(p => p.StationId == request.ToStation).StationOrder;
                                changedStationOrder = tempRailScheduals.Single(p => p.StationId == automationNotification.ChangedStationId).StationOrder;
                                hasFound = changedStationOrder >= fromStationOrder && changedStationOrder <= toStationOrder;
                            }
                            else if (trainStations.Contains(request.FromStation) && trainStations.Contains((int)automationNotification.ChangedStationId))
                            {
                                                               fromStationOrder = tempRailScheduals.Single(p => p.StationId == request.FromStation).StationOrder;
                                changedStationOrder = tempRailScheduals.Single(p => p.StationId == automationNotification.ChangedStationId).StationOrder;
                                hasFound = changedStationOrder >= fromStationOrder;
                            }
                            else if (trainStations.Contains(request.ToStation) && trainStations.Contains((int)automationNotification.ChangedStationId))
                            {
                                                               toStationOrder = tempRailScheduals.Single(p => p.StationId == request.ToStation).StationOrder;
                                changedStationOrder = tempRailScheduals.Single(p => p.StationId == automationNotification.ChangedStationId).StationOrder;
                                hasFound = changedStationOrder <= toStationOrder;
                            }
                            else
                            {
                                                               hasFound = true;
                            }

                            if (hasFound)
                            {
                                travel.TravelMessages.Add(new()
                                {
                                    TrainNumber = automationNotification.TrainNumber,
                                    Message = GetMessage(configurations, stations, automationNotification, train),
                                    Title = notificationTypes.Single(p => p.Id == automationNotification.NotificationTypeId).Description,
                                    Sevirity = (int)WarningTypes.Informative
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("set station order failed", ex);
                                                   }
                    }
                }
                else
                {
                    _logger.LogWarning($"train numbers: {trains} not in table RailSchedual");
                                   }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("set travel message failed", ex);
                   }
    }
    private static string GetMessageBody(TrainWarning trainWarning, Languages languageId)
    {
        if (languageId == Languages.Hebrew)
        {
            return trainWarning.MessageBodyHebrew;
        }
        else if (languageId == Languages.English)
        {
            return trainWarning.MessageBodyEnglish;
        }
        else if (languageId == Languages.Arabic)
        {
            return trainWarning.MessageBodyArabic;
        }
        else if (languageId == Languages.Russian)
        {
            return trainWarning.MessageBodyRussian;
        }
        else
        {
            return string.Empty;
        }
    }
    private static string GetMessageTitle(TrainWarning trainWarning, Languages languageId)
    {
        if (languageId == Languages.Hebrew)
        {
            return trainWarning.TitleHebrew;
        }
        else if (languageId == Languages.English)
        {
            return trainWarning.TitleEnglish;
        }
        else if (languageId == Languages.Arabic)
        {
            return trainWarning.TitleArabic;
        }
        else if (languageId == Languages.Russian)
        {
            return trainWarning.TitleRussian;
        }
        else
        {
            return string.Empty;
        }
    }
    private static string GetMessage(IEnumerable<ConfigurationParameter> configurations, IEnumerable<Station> stations, AutomationNotification automationNotification, Train train)
    {
        string message = string.Empty;

        string toStation = stations.Single(p => p.StationId == train.DestinationStation).HebrewName;

        switch ((NotificationTypes)automationNotification.NotificationTypeId)
        {
            case NotificationTypes.AddTrainStop:
                message = configurations.First(p => p.Key == ConfigurationKeys.AddSingleStopMassage).ValueMob;
                string fromStation = stations.Single(p => p.StationId == train.OrignStation).HebrewName;
                string newStation = stations.Single(p => p.StationId == automationNotification.ChangedStationId).HebrewName;
                message = string.Format(message, train.DepartureTime.ToIsraelString(), fromStation, toStation, train.TrainNumber, newStation);
                break;
            case NotificationTypes.CancelTrainStop:
                message = configurations.First(p => p.Key == ConfigurationKeys.CancelSingleStopMassage).ValueMob;
                string cancelStation = stations.Single(p => p.StationId == automationNotification.ChangedStationId).HebrewName;
                message = string.Format(message, train.DepartureTime.ToIsraelString(), toStation, train.TrainNumber, cancelStation);
                break;
        }

        return message;
    }
    private async Task<IEnumerable<AutomationNotification>> GetAutomationNotificationsAsync(DateTime dateTimeToSearch, string trainNumbers)
    {
        IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@date", Value = dateTimeToSearch.Date },
                new SqlParameter { ParameterName = "@trainNumbers", Value = trainNumbers}
        };

        IEnumerable<AutomationNotification> automationNotifications = await _railDbContext.AutomationNotifications
            .FromSqlRaw<AutomationNotification>("exec [dbo].[rjpa_GetTravelMessagesForAutomationNotification] @date, @trainNumbers", sqlParameters.ToArray())
            .ToArrayAsync().ConfigureAwait(false);
        return automationNotifications;
    }
    private async Task<IEnumerable<TrainWarning>> GetTrainWarningsAsync(DateTime dateTimeToSearch, string trainNumbers)
    {
        IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@date", Value = dateTimeToSearch },
                new SqlParameter { ParameterName = "@trainNumbers", Value = trainNumbers}
        };

        IEnumerable<TrainWarning> trainWarnings = await _railDbContext.TrainWarnings
            .FromSqlRaw<TrainWarning>("exec [dbo].[rjpa_GetTravelMessagesForTravelWarning] @date, @trainNumbers", sqlParameters.ToArray())
            .ToArrayAsync().ConfigureAwait(false);
        return trainWarnings;
    }
    private async Task<IEnumerable<RailSchedual>> GetRailSchedualAsync(DateTime dateTimeToSearch, string trainNumbers)
    {
        IList<SqlParameter> sqlParameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@date", Value = dateTimeToSearch.Date },
                new SqlParameter { ParameterName = "@trainNumbers", Value = trainNumbers}
        };

        IEnumerable<RailSchedual> railScheduals = await _railDbContext.RailScheduals
            .FromSqlRaw<RailSchedual>("exec [dbo].[rjpa_GetRailSchedual] @date, @trainNumbers", sqlParameters.ToArray())
            .ToArrayAsync().ConfigureAwait(false);
        return railScheduals;
    }
    private async Task SetTravelFreeSeatsAsync(IEnumerable<Travel> travels, TrainTimeTableRequest request, DateTime requestDate)
    {
        try
        {
            IEnumerable<int> trainNumbers = travels.Where(travel => travel.DepartureTime.Date == requestDate.Date)
                        .SelectMany(travel => travel.Trains.Select(t => t.TrainNumber).ToArray())
                        .GroupBy(p => p).Select(p => p.Key)
                        .ToArray();

            FreeSeats freeSeats = await _freeSeatsService.GetFreeSeatsAsync(new FreeSeatsRequest
            {
                TrainDate = requestDate.ToString("yyyy-MM-dd"),
                OriginStation = request.FromStation,
                DestinationStation = request.ToStation,
                TrainNumbers = trainNumbers
            }).ConfigureAwait(false);

            if (freeSeats != null)
            {
                _ = travels.Where(travel => travel.DepartureTime.Date == requestDate.Date)
                        .Select(travel =>
                        {
                            _ = travel.Trains.Select(train => train.FreeSeats = freeSeats.TrainAvailableSeats.SingleOrDefault(p => p.TrainNumber == train.TrainNumber).SeatsAvailable).ToArray();
                            _ = travel.FreeSeats = travel.Trains.First().FreeSeats;
                            return travel;
                        }).ToArray();
            }
            else
            {
                int defaultTrainVouchers = int.Parse(await GetConfigurationItemAsync("DefaultTrainVouchers", request.SystemType).ConfigureAwait(false));

                _ = travels.Where(travel => travel.DepartureTime.Date == requestDate.Date)
                    .Select(travel =>
                    {
                        _ = travel.FreeSeats = defaultTrainVouchers;
                        _ = travel.Trains.Select(train => train.FreeSeats = defaultTrainVouchers).ToArray();
                        return travel;
                    }).ToArray();

                throw new FreeSeatsCommException();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "rjpa with free seats error");
                       throw new FreeSeatsCommException();
        }
    }
}
