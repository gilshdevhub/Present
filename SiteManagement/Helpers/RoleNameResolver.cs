using AutoMapper;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using SiteManagement.Dtos;

namespace SiteManagement.Helpers;

public class RoleNameResolver : IValueResolver<AppUser, UserDto, IEnumerable<string>>
{
    private readonly UserManager<AppUser> _userManager;

    public RoleNameResolver(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }



    public IEnumerable<string> Resolve(AppUser source, UserDto destination, IEnumerable<string> destMember, ResolutionContext context)
    {
        IEnumerable<string> roles = _userManager.GetRolesAsync(source).Result;
        return roles;
    }
}
