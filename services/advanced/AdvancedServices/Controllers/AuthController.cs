using EventBus;
using EventBus.Event;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedServices.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IEventBus eventBus, ILogger<AuthController> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login()
    {
        _logger.LogInformation("Login event triggered.");
        var responses = await _eventBus.PublishAsync<UserCreatedEvent, string>(new UserCreatedEvent
        {
            Email = "test@gmail.com",
            Username = "test"
        });
        
        return Ok(new
        {
            Message = "User creation event published successfully.",
            Responses = responses
        });
    }
}