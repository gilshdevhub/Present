using Core.Entities;

namespace Core.Interfaces;

public interface IKirtusConnetctionService
{
    Task<LoginResponseList> Login(LoginShortRequestDto requestData);

}
