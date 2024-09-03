using Microsoft.AspNetCore.Mvc;

namespace ContentManagement.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion(version: "1.0")]
public abstract class BaseApiController : ControllerBase
{
}

