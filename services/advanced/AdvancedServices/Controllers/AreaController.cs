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
        
        var responses = await _eventBus.PublishAsync<ActionReactionEvent, (string, ResultType)>(new ActionReactionEvent());

        return Ok(responses);
    }
}