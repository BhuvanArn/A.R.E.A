using ActionReactionService;
using AdvancedServices.Request;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Microsoft.AspNetCore.Mvc;
using Action = Database.Entities.Action;

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
    
    private string GetUserTokenFromHeaders()
    {
        return Request.Headers.TryGetValue("X-User-Token", out var token) ? token.ToString() : string.Empty;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Action reaction event triggered.");
        
        var response = await _eventBus.PublishAsync<ActionReactionEvent, (object, ResultType)>(new ActionReactionEvent());
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpGet("services/{getArea}")]
    public async Task<IActionResult> GetServices(bool getArea)
    {
        var userToken = GetUserTokenFromHeaders();

        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation("GetServices event triggered.");

        var response = await _eventBus.PublishAsync<GetServiceEvent, (object, ResultType)>(new GetServiceEvent
        {
            JwtToken = userToken,
            GetArea = getArea
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("subscribe_service")]
    public async Task<IActionResult> SubscribeService([FromBody] SubscribeServiceRequest request)
    {
        var userToken = GetUserTokenFromHeaders();
        
        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation($"SubscribeService event triggered for token: {userToken} and service: {request.Name}", userToken, request.Name);

        var response = await _eventBus.PublishAsync<SubscribeServiceEvent, (string, ResultType)>(new SubscribeServiceEvent
        {
            JwtToken = userToken,
            Name = request.Name,
            Auth = request.Auth
        });

        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }
    
    [HttpPost("unsubscribe_service")]
    public async Task<IActionResult> UnsubscribeService([FromBody] UnsubscribeServiceRequest request)
    {
        var userToken = GetUserTokenFromHeaders();
        
        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation($"UnsubscribeService event triggered for token: {userToken} and service: {request.Name}", userToken, request.Name);

        var response = await _eventBus.PublishAsync<UnsubscribeServiceEvent, (List<Service>, ResultType)>(new UnsubscribeServiceEvent
        {
            JwtToken = userToken,
            Name = request.Name
        });

        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpGet("services/{service_name}/actions_reactions")]
    public async Task<IActionResult> GetActionsReactions(string service_name)
    {
        var userToken = GetUserTokenFromHeaders();
        
        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation($"GetActionsReactions event triggered with token {userToken} and service {service_name}");
        
        var response = await _eventBus.PublishAsync<GetActionsReactionsEvent, (object, ResultType)>(new GetActionsReactionsEvent
        {
            ServiceName = service_name,
            JwtToken = userToken
        });

        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpGet("services/{service_name}/action")]
    public async Task<IActionResult> GetAction(string service_name)
    {
        var userToken = GetUserTokenFromHeaders();
        
        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation("GetActions event triggered");
        
        var response = await _eventBus.PublishAsync<GetActionEvent, (List<Action>, ResultType)>(new GetActionEvent
        {
            ServiceName = service_name,
            JwtToken = userToken
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }
    
    [HttpGet("services/{service_name}/reaction")]
    public async Task<IActionResult> GetReaction(string service_name)
    {
        var userToken = GetUserTokenFromHeaders();
        
        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation("GetReaction event triggered");
        
        var response = await _eventBus.PublishAsync<GetReactionEvent, (List<Reaction>, ResultType)>(new GetReactionEvent
        {
            ServiceName = service_name,
            JwtToken = userToken
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("addactions")]
    public async Task<IActionResult> AddAction([FromBody] AddActionRequest request)
    {
        var userToken = GetUserTokenFromHeaders();
        
        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation("AddActions event triggered");

        var response = await _eventBus.PublishAsync<AddActionEvent, (string, ResultType)>(new AddActionEvent
        {
            ServiceId = request.ServiceId,
            Name = request.Name,
            DisplayName = request.DisplayName,
            TriggerConfig = request.TriggerConfig,
            JwtToken = userToken
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("addreactions")]
    public async Task<IActionResult> AddReaction([FromBody] AddReactionRequest request)
    {
        var userToken = GetUserTokenFromHeaders();
        
        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation("AddReactions event triggered");

        var response = await _eventBus.PublishAsync<AddReactionEvent, (string, ResultType)>(new AddReactionEvent
        {
            ServiceId = request.ServiceId,
            ActionId = request.ActionId,
            Name = request.Name,
            ExecutionConfig = request.ExecutionConfig,
            JwtToken = userToken
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }
    
    [HttpDelete("delete_areas")]
    public async Task<IActionResult> DeleteAreas([FromBody] DeleteAreaRequest request)
    {
        var userToken = GetUserTokenFromHeaders();
        
        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation("DeleteAreas event triggered.");

        var response = await _eventBus.PublishAsync<DeleteAreaEvent, (string, ResultType)>(new DeleteAreaEvent
        {
            JwtToken = userToken,
            ServiceId = request.ServiceId,
            ActionId = request.ActionId,
            ReactionId = request.ReactionId
        });

        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPut("update_service")]
    public async Task<IActionResult> UpdateService([FromBody] UpdateServiceRequest request)
    {
        var userToken = GetUserTokenFromHeaders();

        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation("UpdateService event triggered");

        var response = await _eventBus.PublishAsync<UpdateServiceEvent, (string, ResultType)>(new UpdateServiceEvent
        {
            NewAuth = request.NewAuth,
            JwtToken = userToken,
            ServiceId = request.ServiceId
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }

    [HttpPost("update_area")]
    public async Task<IActionResult> UpdateArea([FromBody] UpdateAreaRequest request)
    {
        var userToken = GetUserTokenFromHeaders();

        if (string.IsNullOrEmpty(userToken))
        {
            return Unauthorized("You are not connected.");
        }
        
        _logger.LogInformation("UpdateArea event triggered");

        var response = await _eventBus.PublishAsync<UpdateAreaEvent, (string, ResultType)>(new UpdateAreaEvent
        {
            AreaId = request.AreaId,
            DisplayName = request.DisplayName,
            State = (AreaState?)request.State,
            JwtToken = userToken
        });
        
        return response.Item2 == ResultType.Fail ? Unauthorized(response.Item1) : Ok(response.Item1);
    }
}