using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class KirtusConnetctionService : IKirtusConnetctionService
{
    private readonly RailDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessengerService> _logger;


    public KirtusConnetctionService(ILogger<MessengerService> logger, RailDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<LoginResponseList> Login(LoginShortRequestDto requestData)
    {
        LoginRequestDto loginRequestDto = new LoginRequestDto();
        loginRequestDto.StationID = _configuration.GetValue<int>("Kirtus:StationID");
        loginRequestDto.MachineTypeID = _configuration.GetValue<int>("Kirtus:MachineTypeID");
        loginRequestDto.MachineNumber = _configuration.GetValue<int>("Kirtus:MachineNumber");
        loginRequestDto.IdentificationCardTypes = _configuration.GetValue<int>("Kirtus:IdentificationCardTypes");
        loginRequestDto.FineID = requestData.FineID;
        loginRequestDto.PersonID = requestData.PersonID;

        LoginResponseList list =  new ();
        LoginResponseDto res = new ();
        list.lstFineDEtailsByPassengerID = new ();
        list.lstFineDEtailsByPassengerID.Add(res);
        return list;
    }


}
