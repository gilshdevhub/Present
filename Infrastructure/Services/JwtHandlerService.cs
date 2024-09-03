using Core.Config;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Infrastructure.Services;

public class JwtHandlerService : IJwtHandlerService
{
    private readonly JwtRsaConfig _settings;

    public JwtHandlerService(IOptions<JwtRsaConfig> settings)
    {
        _settings = settings.Value;
    }

    public JwtResponse CreateToken(IEnumerable<KeyValuePair<string, string>> claims, int otpTimeout)
    {
        var privateKey = Convert.FromBase64String(_settings.RsaPrivateKey);

        using RSA rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privateKey, out _);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha512)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        };

        var now = DateTime.Now;
        var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

        Collection<Claim> myClaims = new()
        {
            new Claim(JwtRegisteredClaimNames.Iat, unixTimeSeconds.ToString(), ClaimValueTypes.Integer64),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var claim in claims)
        {
            myClaims.Add(new Claim(claim.Key, claim.Value));
        }

        var jwt = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: myClaims,
            notBefore: now,
            expires: now.AddMinutes(otpTimeout),
            signingCredentials: signingCredentials
        );

        string token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new JwtResponse
        {
            Token = token,
            ExpiresAt = unixTimeSeconds,
        };
    }

    public bool ValidateToken(string token)
    {
        var publicKey = Convert.FromBase64String(_settings.RsaPublicKey);

        using RSA rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(publicKey, out _);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _settings.Issuer,
            ValidAudience = _settings.Audience,
            IssuerSigningKey = new RsaSecurityKey(rsa),
            CryptoProviderFactory = new CryptoProviderFactory()
            {
                CacheSignatureProviders = false
            }
        };

        try
        {
            var handler = new JwtSecurityTokenHandler();
            _ = handler.ValidateToken(token, validationParameters, out var validatedSecurityToken);
        }
        catch
        {
            return false;
        }

        return true;
    }
}
