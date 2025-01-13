using AdvancedServices.Request;
using EventBus;
using EventBus.Event;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = AdvancedServices.Request.LoginRequest;
using RegisterRequest = AdvancedServices.Request.RegisterRequest;

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

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
    {
        _logger.LogInformation("Google login event triggered.");
        
        var response = await _eventBus.PublishAsync<GoogleLoginEvent, (string, ResultType)>(new GoogleLoginEvent
        {
            Token = request.Token
        });

        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordEventRequest request)
    {
        _logger.LogInformation("Forgot password event triggered.");

        var response = await _eventBus.PublishAsync<ForgotPasswordEvent, (string, ResultType)>(new ForgotPasswordEvent
        {
            Email = request.Email
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        _logger.LogInformation("Reset password event triggered.");
        
        var response = await _eventBus.PublishAsync<UserResetPasswordEvent, (string, ResultType)>(new UserResetPasswordEvent
        {
            JwtToken = request.JwtToken,
            Password = request.Password,
            NewPassword = request.NewPassword
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login event triggered.");
        
        var response = await _eventBus.PublishAsync<UserCreatedEvent, (string, ResultType)>(new UserCreatedEvent
        {
            Email = request.Email,
            Password = request.Password
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Register event triggered.");

        var response = await _eventBus.PublishAsync<UserRegisteredEvent, (string, ResultType)>(new UserRegisteredEvent
        {
            Email = request.Email,
            ConfirmedPassword = request.ConfirmedPassword,
            Password = request.Password,
            Username = request.Username
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }
}