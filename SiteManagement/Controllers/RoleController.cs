using SiteManagement.Dtos;
using AutoMapper;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Interfaces;
using Core.Errors;

namespace SiteManagement.Controllers;

public class RoleController : BaseApiController
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public RoleController(RoleManager<AppRole> roleManager, IMapper mapper,
             IRoleService roleService)
    {
        _roleManager = roleManager;
        _mapper = mapper;
        _roleService = roleService;
    }


    [HttpPost]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> CreateRole(RoleDto roleDto)
    {
        AppRole role = new AppRole
        {
            Name = roleDto.Name
        };

        IdentityResult result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
        }
        if (roleDto.PageRoleNew.Count > 0)
        {
            bool pageResult = await _roleService.AddOrUpdatePageRolesAsync(roleDto.PageRoleNew, role.Id);
            if (!pageResult)
            {
                return BadRequest(new ApiErrorResponse { Message = "שמירה נכשלה", StatusCode = 400 });
            }
        }
        return Ok(true);
    }


    [HttpPut]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<bool>> UpdateRole(RoleDto roleDto)
    {

        if (roleDto.PageRoleNew.Count > 0)
        {
            bool pageResult = await _roleService.AddOrUpdatePageRolesAsync(roleDto.PageRoleNew, roleDto.Id);
            if (!pageResult)
            {
                return BadRequest(new ApiErrorResponse { Message = "שמירת השינוי נכשלה", StatusCode = 400 });
            }
        }
        return Ok(true);
    }

    [HttpGet]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
    {
        IEnumerable<AppRole> roles = await _roleService.GetRolesAsync();
        IEnumerable<RoleDto> roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
        return Ok(roleDtos);
    }

    [HttpDelete("{Id}")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<IdentityResult>> DeleteRole(int Id)
    {
        var error = new ClientErrorData();
        IdentityResult result;
        try
        {
            var role = await _roleManager.FindByIdAsync(Id.ToString());
             result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiErrorResponse { Message = result.Errors.FirstOrDefault().Description, StatusCode = 400 });
            }
        }
        catch
        {
            return BadRequest(new ApiErrorResponse { Message = "מחיקה נכשלה", StatusCode = 400 });
        }

        return Ok(result);
    }
}
