using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class UpdateActionEventHandler : IIntegrationEventHandler<UpdateActionEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public UpdateActionEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(UpdateActionEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not connected", ResultType.Fail);
        }

        var action = (await _dbHandler.GetAsync<Database.Entities.Action>(s => s.Id == @event.ActionId)).FirstOrDefault();

        if (action is null)
        {
            return ("You can't update a non-existant action", ResultType.Fail);
        }
        
        var service = (await _dbHandler.GetAsync<Service>(s => s.Id == action.ServiceId && s.UserId == userId)).FirstOrDefault();

        if (service is null)
        {
            return ("You can't update a non-existant action", ResultType.Fail);
        }
        
        action.TriggerConfig = @event.TriggerConfig;
        await _dbHandler.UpdateAsync(action);
        
        return ("Ok", ResultType.Success);
    }
}