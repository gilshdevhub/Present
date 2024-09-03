using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace PisData.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion(version: "1.0")]
public abstract class BaseApiController : ControllerBase
{
}
