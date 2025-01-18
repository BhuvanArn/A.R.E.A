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
            var reactions = await _dbHandler.GetAsync<Reaction>(r => r.ServiceId == service.Id);

            var serviceDto = new ServiceWithAuth
            {
                Id = service.Id,
                Name = service.Name,
                UserId = service.UserId,
                Auth = TryParseJson(service.Auth),
                Actions = actions.Select(a => new Action
                {
                    Id = a.Id,
                    Name = a.Name,
                    ServiceId = a.ServiceId,
                    TriggerConfig = a.TriggerConfig
                }).ToList(),
                Reactions = reactions.Select(r => new Reaction
                {
                    Id = r.Id,
                    Name = r.Name,
                    ActionId = r.ActionId,
                    ServiceId = r.ServiceId,
                    ExecutionConfig = r.ExecutionConfig
                }).ToList()
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
        
        public List<Action> Actions { get; set; } = new();
        public List<Reaction> Reactions { get; set; } = new();
    }
}