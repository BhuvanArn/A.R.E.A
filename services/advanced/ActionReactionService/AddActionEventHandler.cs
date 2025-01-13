using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class AddActionEventHandler : IIntegrationEventHandler<AddActionEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public AddActionEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(AddActionEvent @event)
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

        var action = new Database.Entities.Action
        {
            ServiceId = @event.ServiceId,
            Name = @event.Name,
            TriggerConfig = @event.TriggerConfig
        };
        
        await _dbHandler.AddAsync(action);
        return ("Ok", ResultType.Success);
    }
}