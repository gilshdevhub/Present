using Core.Entities.Compensation;

namespace Core.Interfaces;

public interface ICompensationService
{
    Task<Compensation> CreateCompensationAsync(Compensation compensation);
    Task<IEnumerable<Compensation>> SearchCompensationsAsync(SearchCompensation searchCompensation);
    Task<IEnumerable<Compensation>> SearchOtpCompensationsAsync(SearchCompensation searchCompensation);
    Task<CompensationOtpResponse> CreateOtpAsync(CompensationOtpRequest compensationOtpRequest);
}
