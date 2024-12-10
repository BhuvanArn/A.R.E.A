using AdvancedServices.Request;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedServices.Controllers;

[ApiController]
[Route("[controller]")]
public class AreaController : ControllerBase
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<AreaController> _logger;

    public AreaController(IEventBus eventBus, ILogger<AreaController> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Action reaction event triggered.");
        
        var responses = await _eventBus.PublishAsync<ActionReactionEvent, (List<Service>, ResultType)>(new ActionReactionEvent());

        return Ok(responses.Select(s => s.Item1));
    }

    [HttpGet("{user_token}/services")]
    public async Task<IActionResult> GetServices(string user_token)
    {
        _logger.LogInformation("GetServices event triggered.");

        var responses = await _eventBus.PublishAsync<GetServiceEvent, (List<Service>, ResultType)>(new GetServiceEvent
        {
            JwtToken = user_token
        });
        
        return Ok(responses.Select(s => s.Item1));
    }

    [HttpPost("{user_token}/subscribe_service")]
    public async Task<IActionResult> SubscribeService(string user_token, [FromBody] SubscribeServiceRequest request)
    {
        _logger.LogInformation("SubscribeService event triggered for token: {UserToken} and service: {ServiceName}", user_token, request.Name);

        var responses = await _eventBus.PublishAsync<SubscribeServiceEvent, (List<Service>, ResultType)>(new SubscribeServiceEvent
        {
            JwtToken = user_token,
            Name = request.Name,
            Credentials = request.Credentials
        });

        return Ok();
    }
}