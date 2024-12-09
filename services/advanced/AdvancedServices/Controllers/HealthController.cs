using Microsoft.AspNetCore.Mvc;

namespace AdvancedServices.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("OK");
}