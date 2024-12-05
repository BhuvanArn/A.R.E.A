using AdvancedServices.Request;
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login event triggered.");
        
        var responses = await _eventBus.PublishAsync<UserCreatedEvent, (string, ResultType)>(new UserCreatedEvent
        {
            Email = request.Email,
            Password = request.Password
        });
        
        return Ok(new
        {
            Message = "User login event published successfully.",
            Responses = responses.Select(s => s.Item1)
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Register event triggered.");

        var responses = await _eventBus.PublishAsync<UserRegisteredEvent, (string, ResultType)>(new UserRegisteredEvent
        {
            Email = request.Email,
            ConfirmedPassword = request.ConfirmedPassword,
            Password = request.Password,
            Username = request.Username
        });

        return Ok(new
        {
            Message = "User registration event published successfully.",
            Responses = responses.Select(s => s.Item1)
        });
    }
}