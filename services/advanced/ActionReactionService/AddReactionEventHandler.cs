using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class AddReactionEventHandler : IIntegrationEventHandler<AddReactionEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public AddReactionEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(AddReactionEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not connected", ResultType.Fail);
        }

        var service = (await _dbHandler.GetAsync<Service>(s => s.Id == @event.ServiceId)).FirstOrDefault();

        if (service == null || service.UserId != userId)
        {
            return ("Service cannot be found", ResultType.Fail);
        }
        
        var action = (await _dbHandler.GetAsync<Action>(s => s.Id == @event.ActionId)).FirstOrDefault();

        if (action == null || action.ServiceId != service.Id)
        {
            return ("Action cannot be found", ResultType.Fail);
        }

        var reaction = new Reaction
        {
            ServiceId = @event.ServiceId,
            ActionId = @event.ActionId,
            Name = @event.Name,
            ExecutionConfig = @event.ExecutionConfig
        };
        
        var addedReaction = await _dbHandler.AddAsync(reaction);
        
        var area = (await _dbHandler.GetAsync<Area>(s => s.ServiceId == @event.ServiceId && s.ActionId == @event.ActionId)).FirstOrDefault();

        if (area is null)
        {
            return ("Ok", ResultType.Success);
        }
        
        area.ReactionId = addedReaction.Id;
        await _dbHandler.UpdateAsync(area);

        return ("Ok", ResultType.Success);
    }
}