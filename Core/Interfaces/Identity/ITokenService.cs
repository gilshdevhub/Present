using Core.Entities.Identity;

namespace Core.Interfaces.Identity;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
