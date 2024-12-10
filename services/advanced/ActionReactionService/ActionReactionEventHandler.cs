using System.Text.Json;
using Database.Entities;
using Database.Service;
using EventBus;
using EventBus.Event;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class ActionReactionEventHandler : IIntegrationEventHandler<ActionReactionEvent, (List<Service>, ResultType)>
{
    private readonly ServiceService _serviceService;
    private readonly ActionService _actionService;
    private readonly ReactionService _reactionService;
    
    public ActionReactionEventHandler(
        ServiceService serviceService,
        ActionService actionService,
        ReactionService reactionService)
    {
        _serviceService = serviceService;
        _actionService = actionService;
        _reactionService = reactionService;
    }
    
    public async Task<(List<Service>, ResultType)> HandleAsync(ActionReactionEvent @event)
    {
        var services = await _serviceService.GetAllServicesAsync();
        
        var result = new List<Service>();
        foreach (var service in services)
        {
            var actions = await _actionService.FindActionsAsync(a => a.ServiceId == service.Id);
            var reactions = await _reactionService.FindReactionsAsync(r => r.ServiceId == service.Id);

            var serviceDto = new Service
            {
                Id = service.Id,
                Name = service.Name,
                UserId = service.UserId,
                Auth = service.Auth,
                Actions = actions.Select(a => new Action
                {
                    Id = a.Id,
                    Name = a.Name,
                    TriggerConfig = a.TriggerConfig
                }).ToList(),
                Reactions = reactions.Select(r => new Reaction
                {
                    Id = r.Id,
                    Name = r.Name,
                    ExecutionConfig = r.ExecutionConfig
                }).ToList()
            };

            result.Add(serviceDto);
        }

        return (result, ResultType.Success);
    }
}