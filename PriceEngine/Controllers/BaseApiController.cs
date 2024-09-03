using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace PriceEngine.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion(version: "1.0")]
public class BaseApiController : ControllerBase
{
}
