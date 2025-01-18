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
        var services = await _dbHandler.GetAllAsync<Service>();
    
        var result = new List<ServiceWithAuth>();
        foreach (var service in services)
        {
            var actions = await _dbHandler.GetAsync<Action>(a => a.ServiceId == service.Id);

            var actionsWithTriggerConfigTasks = actions.Select(async a => 
            {
                var reactions = await _dbHandler.GetAsync<Reaction>(r => r.ActionId == a.Id);
                var actionWithTriggerConfig = new ActionWithTriggerConfig
                {
                    Id = a.Id,
                    Name = a.Name,
                    ServiceId = a.ServiceId,
                    TriggerConfig = TryParseJson(a.TriggerConfig),
                    Reaction = reactions.Select(r => new ReactionWithExecutionConfig
                    {
                        Id = r.Id,
                        ServiceId = r.ServiceId,
                        ActionId = r.ActionId,
                        Action = a,
                        Service = service,
                        Name = r.Name,
                        ExecutionConfig = TryParseJson(r.ExecutionConfig)
                    }).ToList()
                };
                return actionWithTriggerConfig;
            }).ToList();

            var actionsWithTriggerConfig = await Task.WhenAll(actionsWithTriggerConfigTasks);

            var serviceDto = new ServiceWithAuth
            {
                Id = service.Id,
                Name = service.Name,
                UserId = service.UserId,
                Auth = TryParseJson(service.Auth),
                Actions = actionsWithTriggerConfig.ToList(),
            };

            result.Add(serviceDto);
        }

        return (result, ResultType.Success);
    }
    
    private JObject? TryParseJson(string jsonString)
    {
        try
        {
            return string.IsNullOrEmpty(jsonString) ? new JObject() : JObject.Parse(jsonString);
        }
        catch
        {
            return new JObject();
        }
    }
    
    public class ServiceWithAuth
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UserId { get; set; }
        
        public JObject Auth { get; set; } 
        
        public List<ActionWithTriggerConfig> Actions { get; set; } = new();
    }
    
    public class ActionWithTriggerConfig
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    
        public Guid ServiceId { get; set; }
    
        public Service Service { get; set; }
    
        public string Name { get; set; }
    
        public JObject TriggerConfig { get; set; }

        public List<ReactionWithExecutionConfig> Reaction { get; set; } = new();
    }

    public class ReactionWithExecutionConfig
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    
        public Guid ServiceId { get; set; }
    
        public Guid ActionId { get; set; }
    
        public Action Action { get; set; }
    
        public Service Service { get; set; }
    
        public string Name { get; set; }
    
        public JObject ExecutionConfig { get; set; }
    }
}