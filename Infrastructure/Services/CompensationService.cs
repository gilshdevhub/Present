using Core.Entities;
using Core.Entities.Compensation;
using Core.Entities.Configuration;
using Core.Entities.Messenger;
using Core.Entities.Vouchers;
using Core.Enums;
using Core.Helpers;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CompensationService : ICompensationService
{
    private readonly RailDbContext _context;
    private readonly IJwtHandlerService _jwtHandlerService;
    private readonly IMessengerService _messengerService;
    private readonly IStationsService _stationsService;
    private readonly IConfigurationService _configurationService;

    public CompensationService(RailDbContext context, IJwtHandlerService jwtHandlerService, IMessengerService messengerService, IStationsService stationsService,
        IConfigurationService configurationService)
    {
        _context = context;
        _jwtHandlerService = jwtHandlerService;
        _messengerService = messengerService;
        _stationsService = stationsService;
        _configurationService = configurationService;
    }

    public async Task<Compensation> CreateCompensationAsync(Compensation compensation)
    {
        IEnumerable<Station> stations = await _stationsService.GetStationsAsync().ConfigureAwait(false);

        compensation.OriginStationId = stations.Single(s => s.TicketStationId == compensation.OriginStationId).StationId;
        compensation.DestinationStationId = stations.Single(s => s.TicketStationId == compensation.DestinationStationId).StationId;

        var entity = await _context.Compensations.AddAsync(compensation).ConfigureAwait(false);
        Compensation newCompensation = entity.Entity;

        _ = await _context.SaveChangesAsync().ConfigureAwait(false);
        return newCompensation;
    }

    public async Task<IEnumerable<Compensation>> SearchCompensationsAsync(SearchCompensation searchCompensation)
    {
        IQueryable<Compensation> compensationsQuery = null;

        if (searchCompensation.FromDate.HasValue)
        {
            compensationsQuery = _context.Compensations.Where(p => p.CardrecievedDate >= searchCompensation.FromDate);
        }

        if (searchCompensation.ToDate.HasValue)
        {
            compensationsQuery = _context.Compensations.Where(p => p.CardrecievedDate <= searchCompensation.ToDate);
        }

        IEnumerable<Compensation> compensations = await compensationsQuery.ToArrayAsync().ConfigureAwait(false);

        if (compensations.Any())
        {
            IEnumerable<Station> stations = await _stationsService.GetStationsAsync().ConfigureAwait(false);
            foreach (Compensation compensation in compensations)
            {
                compensation.OriginStationId = stations.Single(s => s.StationId == compensation.OriginStationId).TicketStationId;
                compensation.DestinationStationId = stations.Single(s => s.StationId == compensation.DestinationStationId).TicketStationId;
            }
        }

        return compensations;
    }

    public async Task<CompensationOtpResponse> CreateOtpAsync(CompensationOtpRequest compensationOtpRequest)
    {
        CompensationOtpResponse compensationOtpResponse = new();

        IEnumerable<ConfigurationParameter> configurations = await _configurationService.GetAllItemsAsync().ConfigureAwait(false);

        int otpNumberOfDigits = int.Parse(configurations.First(p => p.Key == ConfigurationKeys.OtpNumberOfDigits).ValueMob);
        string otp = new System.Random().Next(1, (int)System.Math.Pow(10, otpNumberOfDigits) - 1).ToString().PadLeft(otpNumberOfDigits, '0');

        IEnumerable<KeyValuePair<string, string>> claims = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>(nameof(compensationOtpRequest.PhoneNumber), compensationOtpRequest.PhoneNumber),
            new KeyValuePair<string, string>("Otp", otp)
        };

        int otpTimeOut = int.Parse(configurations.First(p => p.Key == ConfigurationKeys.OtpTimeOut).ValueMob);

        JwtResponse jwtResponse = _jwtHandlerService.CreateToken(claims, otpTimeOut);

        string otpMessage = string.Format(configurations.First(p => p.Key == ConfigurationKeys.CompensationOtpMessage).ValueMob, otp, otpTimeOut.ToString());
              
        bool hasSent = await _messengerService.SendUniCellSmsAsync(new MessageInfo { Keys = new string[] { compensationOtpRequest.PhoneNumber }, Message = otpMessage, MessageType = MessageTypes.SMS })
            .ConfigureAwait(false);
        if (hasSent)
        {
            compensationOtpResponse = new CompensationOtpResponse { Token = jwtResponse.Token };
            _ = await _context.Otps.AddAsync(new Otp { OtpCode = otp, PhoneNumber = compensationOtpRequest.PhoneNumber, SystemName = "Compensations" }).ConfigureAwait(false);
            _ = await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        return compensationOtpResponse;
    }

    public async Task<IEnumerable<Compensation>> SearchOtpCompensationsAsync(SearchCompensation searchCompensation)
    {
        IEnumerable<Compensation> compensations = await _context.Compensations.Where(c => c.PhoneNumber == searchCompensation.PhoneNumber).ToArrayAsync()
            .ConfigureAwait(false);

        return compensations;
    }
}
