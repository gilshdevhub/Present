using SiteManagement.Dtos;
using AutoMapper;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Entities.PagesManagement;
using Core.Errors;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SiteManagement.Controllers;

public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPagesService _pagesService;
    private readonly IMapper _mapper;

    public AccountController(UserManager<AppUser> userManager, IMapper mapper,
            IPagesService pagesService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _pagesService = pagesService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);

        if (user == null)
        {
            return Unauthorized(new ApiErrorResponse { Message = "שם משתמש או סיסמא שגויים" });
        }

        bool passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!passwordValid)
        {
            return Unauthorized(new ApiErrorResponse { Message = "שם משתמש או סיסמא שגויים" });
        }

        UserDto userDto = _mapper.Map<AppUser, UserDto>(user);
        IEnumerable<PageResponse> pages = await _pagesService.GetPagesPerUserAsync(user.UserName);
        userDto.Pages = pages;
        return Ok(userDto);
    }

    [HttpPost("register")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<UserDto>>> Register(RegisterDto registerDto)
    {
        AppUser user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            UserName = registerDto.UserName,
            Email = registerDto.UserName,
            PhoneNumber = registerDto.PhoneNumber
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
        }

        result = await _userManager.AddToRolesAsync(user, registerDto.RoleNames);

        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
        }

        IEnumerable<AppUser> users = await _userManager.Users.ToArrayAsync();

        IEnumerable<UserDto> usersDto = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserDto>>(users);

        return Ok(usersDto);
    }

    [HttpPut("updateUser")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<UserDto>>> updateUser(RegisterDto registerDto)
    {
        AppUser user = await _userManager.FindByEmailAsync(registerDto.UserName);

        if (user == null)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשל", StatusCode = 400 });
        }
        user.DisplayName = registerDto.DisplayName;
        user.PhoneNumber = registerDto.PhoneNumber;
        

        IdentityResult result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        UserDto userDto = _mapper.Map<AppUser, UserDto>(user);

        result = await _userManager.RemoveFromRolesAsync(user, userDto.Roles);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        result = await _userManager.AddToRolesAsync(user, registerDto.RoleNames);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        result = await _userManager.RemovePasswordAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        result = await _userManager.AddPasswordAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        IEnumerable<AppUser> users = await _userManager.Users.ToArrayAsync();

        IEnumerable<UserDto> usersDto = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserDto>>(users);

        return Ok(usersDto);
    }

    [HttpPut("UpdateRolesForUser")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<UserDto>>> ChangeUserRoles(UserRoleDto userDto)
    {
        AppUser user = await _userManager.FindByEmailAsync(userDto.UserName);

        UserDto dbUserDto = _mapper.Map<AppUser, UserDto>(user);

        IdentityResult result = await _userManager.RemoveFromRolesAsync(user, dbUserDto.Roles);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
        }

        result = await _userManager.AddToRolesAsync(user, userDto.Roles);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
        }
        IEnumerable<AppUser> users = await _userManager.Users.ToArrayAsync();

        IEnumerable<UserDto> usersDto = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserDto>>(users);
        return Ok(usersDto);
    }
    [HttpPut("ChangePassword")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> ChangePasswordUser(LoginDto userDto)
    {
        AppUser user = await _userManager.FindByEmailAsync(userDto.UserName);

        if (user == null)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }

        IdentityResult result = await _userManager.RemovePasswordAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
        }

        result = await _userManager.AddPasswordAsync(user, userDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
        }
        return Ok(true);
    }

    [HttpPut("updateUserDetails")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<UserDto>>> updateUserDetails(UserDto userDto)
    {
        AppUser user = await _userManager.FindByEmailAsync(userDto.UserName);

        if (user == null)
        {
            return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
        }
        user.DisplayName = userDto.DisplayName;
        user.PhoneNumber = userDto.PhoneNumber;
            
        _ = await _userManager.UpdateAsync(user);


                            
        IEnumerable<AppUser> users = await _userManager.Users.ToArrayAsync();

        IEnumerable<UserDto> usersDto = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserDto>>(users);

        return Ok(usersDto);
    }

    [HttpDelete("delete")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<UserDto>>> deleteUser(UserDto userDto)
    {
        AppUser user = await _userManager.FindByEmailAsync(userDto.UserName);

        if (user == null)
        {
            return BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }

        IdentityResult result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
        }

        IEnumerable<AppUser> users = await _userManager.Users.ToArrayAsync();

        IEnumerable<UserDto> usersDto = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserDto>>(users);

        return Ok(usersDto);
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        IEnumerable<AppUser> users = await _userManager.Users.ToArrayAsync();

        IEnumerable<UserDto> usersDto = _mapper.Map<IEnumerable<AppUser>, IEnumerable<UserDto>>(users);

        return Ok(usersDto);
    }
}
