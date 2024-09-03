using Core.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace API.Extensions;

public static class AuthenticationServiceExtension
{
    public static void ConfigureAutenticationService(this IServiceCollection services, JwtRsaConfig jwtRsaConfig)
    {
        services.AddSingleton<RsaSecurityKey>(_ => {
                      
            RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(
                source: Convert.FromBase64String(jwtRsaConfig.RsaPublicKey),
                bytesRead: out int _
            );

            return new RsaSecurityKey(rsa);
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(config =>
            {
                SecurityKey rsa = services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();

                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = rsa,
                    ValidAudience = jwtRsaConfig.Audience,
                    ValidIssuer = jwtRsaConfig.Issuer,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true
                };
            });
    }
}
