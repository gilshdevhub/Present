using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SiteManagement.Controllers;

public class ManagmentSystemObjectsController : BaseApiController
{
    private readonly IManagmentSystemObjects _managmentSystemObjects;
    public ManagmentSystemObjectsController(IManagmentSystemObjects managmentSystemObjects)
    {
        _managmentSystemObjects = managmentSystemObjects;
    }
    [HttpGet("GetManagmentObjects")]
    [Authorize(Policy = "PageRole")]
    public async Task<List<ManagmentSystemObjects>> GetManagmentObjects()
    {
        var objects = await _managmentSystemObjects.GetManagmentObjectsAsync().ConfigureAwait(false);
        return objects;
    }
    [HttpGet("GetManagmentObjectsById")]
    [Authorize(Policy = "PageRole")]
    public async Task<ManagmentSystemObjects> GetManagmentObjectsById(int Id)
    {
        var objects = await _managmentSystemObjects.GetManagmentObjectsByIdAsync(Id).ConfigureAwait(false);
        return objects;
    }
    [HttpGet("GetManagmentObjectsByName")]
    [Authorize(Policy = "PageRole")]
    public async Task<ManagmentSystemObjects> GetManagmentObjectsByName(string Name)
    {
        var objects = await _managmentSystemObjects.GetManagmentObjectsByNameAsync(Name).ConfigureAwait(false);
        return objects;
    }
    [HttpPut("UpdateManagmentObjects")]
    [Authorize(Policy = "PageRole")]
    public async Task<ActionResult<ManagmentSystemObjects>> UpdateManagmentObjects(int Id, string value)
    {
               var objects = await _managmentSystemObjects.UpdateManagmentObjectsByIdAsync(Id, value).ConfigureAwait(false);
        return objects;
    }
}
