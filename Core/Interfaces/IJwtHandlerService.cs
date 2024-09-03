using Core.Entities;

namespace Core.Interfaces;

public interface IJwtHandlerService
{
    JwtResponse CreateToken(IEnumerable<KeyValuePair<string, string>> claims, int otpTimeout);
    bool ValidateToken(string token);
}
