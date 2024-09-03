using Core.Entities.Fares;

namespace Core.Interfaces;

public interface IFaresService
{
    Task<FaresData> GetFares(FaresRequestData requestData);
    Task<CodesDataDto> GetCodesData();


}
