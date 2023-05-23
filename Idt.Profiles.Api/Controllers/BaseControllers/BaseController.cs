using Microsoft.AspNetCore.Mvc;

namespace Idt.Profiles.Api.Controllers.BaseControllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    
}