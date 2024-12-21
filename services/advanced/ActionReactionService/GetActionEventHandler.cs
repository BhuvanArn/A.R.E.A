using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class GetActionEventHandler : IIntegrationEventHandler<GetActionEvent, (List<Action>, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public GetActionEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(List<Action>, ResultType)> HandleAsync(GetActionEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var service = (await _dbHandler.GetAsync<Service>(s => s.UserId == userId && s.Name == @event.ServiceName)).FirstOrDefault();

        if (service == null)
        {
            return (new(), ResultType.Fail);
        }
        
        var actions = await _dbHandler.GetAsync<Action>(s => s.ServiceId == service.Id);
        
        return (actions.ToList(), ResultType.Success);
    }
}