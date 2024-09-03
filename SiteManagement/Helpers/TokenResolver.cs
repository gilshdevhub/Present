using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces.Identity;
using SiteManagement.Dtos;

namespace SiteManagement.Helpers;
public class TokenResolver : IValueResolver<AppUser, UserDto, string>
{
    private readonly ITokenService _tokenService;

    public TokenResolver(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public string Resolve(AppUser source, UserDto destination, string destMember, ResolutionContext context)
    {
        return _tokenService.CreateToken(source);
    }
}
