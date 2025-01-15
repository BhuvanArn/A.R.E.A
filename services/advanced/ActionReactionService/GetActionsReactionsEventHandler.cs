using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class GetActionsReactionsEventHandler : IIntegrationEventHandler<GetActionsReactionsEvent, (object, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public GetActionsReactionsEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(object, ResultType)> HandleAsync(GetActionsReactionsEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var service = (await _dbHandler.GetAsync<Service>(s => s.UserId == userId && s.Name == @event.ServiceName)).FirstOrDefault();

        if (service is null)
        {
            return (new(), ResultType.Fail);
        }
        
        var actions = await _dbHandler.GetAsync<Action>(a => a.ServiceId == service.Id);
        var reactions = await _dbHandler.GetAsync<Reaction>(r => r.ServiceId == service.Id, r => r.Action);

        var response = actions.Select(action => new
        {
            ActionName = action.Name,
            Description = action.TriggerConfig,
            Reactions = reactions
                .Where(reaction => reaction.ActionId == action.Id)
                .Select(reaction => new
                {
                    ReactionName = reaction.Name,
                    ActionName = reaction.Action?.Name ?? "Unknown Action",
                    Description = reaction.ExecutionConfig
                }).ToList()
        }).ToList();

        return (response, ResultType.Success);
    }
}