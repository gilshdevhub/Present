namespace Core.Entities;

public class JwtResponse
{
    public string Token { get; set; }
    public long ExpiresAt { get; set; }
}
