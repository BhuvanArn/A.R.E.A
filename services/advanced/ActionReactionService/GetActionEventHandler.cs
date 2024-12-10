using Database.Service;
using EventBus;
using EventBus.Event;
using Extension;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class GetActionEventHandler : IIntegrationEventHandler<GetActionEvent, (List<Action>, ResultType)>
{
    private readonly ServiceService _serviceService;
    private readonly ActionService _actionService;

    public GetActionEventHandler(ServiceService serviceService, ActionService actionService)
    {
        _serviceService = serviceService;
        _actionService = actionService;
    }
    
    public async Task<(List<Action>, ResultType)> HandleAsync(GetActionEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var service = (await _serviceService.FindServicesAsync(s => s.UserId == userId && s.Name == @event.ServiceName)).FirstOrDefault();

        if (service == null)
        {
            return (new(), ResultType.Fail);
        }
        
        var actions = await _actionService.FindActionsAsync(s => s.ServiceId == service.Id);
        
        return (actions.ToList(), ResultType.Success);
    }
}