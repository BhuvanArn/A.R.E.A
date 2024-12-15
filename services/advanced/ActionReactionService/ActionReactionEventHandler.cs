using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class ActionReactionEventHandler : IIntegrationEventHandler<ActionReactionEvent, (List<Service>, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    
    public ActionReactionEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(List<Service>, ResultType)> HandleAsync(ActionReactionEvent @event)
    {
        var services = await _dbHandler.GetAllAsync<Service>();
        
        var result = new List<Service>();
        foreach (var service in services)
        {
            var actions = await _dbHandler.GetAsync<Action>(a => a.ServiceId == service.Id);
            var reactions = await _dbHandler.GetAsync<Reaction>(r => r.ServiceId == service.Id);

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
                    ActionId = r.ActionId,
                    ExecutionConfig = r.ExecutionConfig
                }).ToList()
            };

            result.Add(serviceDto);
        }

        return (result, ResultType.Success);
    }
}