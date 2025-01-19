using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Newtonsoft.Json.Linq;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class ActionReactionEventHandler : IIntegrationEventHandler<ActionReactionEvent, (object, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    
    public ActionReactionEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(object, ResultType)> HandleAsync(ActionReactionEvent @event)
    {
        var result = new List<ServiceWithAuth>();

        var services = await _dbHandler.GetAllAsync<Service>();

        foreach (var service in services)
        {
            var actions = await _dbHandler.GetAsync<Action>(a => a.ServiceId == service.Id);

            var reactions = await _dbHandler.GetAsync<Reaction>(r => r.ServiceId == service.Id);

            var serviceDto = new ServiceWithAuth
            {
                Id = service.Id,
                Name = service.Name,
                UserId = service.UserId,
                Auth = service.Auth,
                Actions = actions.Select(a => new ActionWithTriggerConfig
                {
                    Id = a.Id,
                    Name = a.Name,
                    ServiceId = a.ServiceId,
                    TriggerConfig = a.TriggerConfig
                }).ToList(),
                Reactions = reactions.Select(r => new ReactionWithExecutionConfig
                {
                    Id = r.Id,
                    ServiceId = r.ServiceId,
                    ActionId = r.ActionId,
                    Name = r.Name,
                    ExecutionConfig = r.ExecutionConfig
                }).ToList()
            };

            result.Add(serviceDto);
        }

        return (result, ResultType.Success);
    }

    public class ServiceWithAuth
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public Guid UserId { get; set; }
        
        public string Auth { get; set; } 
        
        public List<ActionWithTriggerConfig> Actions { get; set; } = new();
        
        public List<ReactionWithExecutionConfig> Reactions { get; set; } = new();
    }
    
    public class ActionWithTriggerConfig
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    
        public Guid ServiceId { get; set; }
    
        public string Name { get; set; }
    
        public string TriggerConfig { get; set; }
    }

    public class ReactionWithExecutionConfig
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    
        public Guid ServiceId { get; set; }
    
        public Guid ActionId { get; set; }
    
        public string Name { get; set; }
    
        public string ExecutionConfig { get; set; }
    }
}
