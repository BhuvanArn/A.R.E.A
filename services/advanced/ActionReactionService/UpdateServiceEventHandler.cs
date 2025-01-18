using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class UpdateServiceEventHandler : IIntegrationEventHandler<UpdateServiceEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public UpdateServiceEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(UpdateServiceEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not connected", ResultType.Fail);
        }
        
        var service = (await _dbHandler.GetAsync<Service>(s => s.Id == @event.ServiceId && s.UserId == userId)).FirstOrDefault();

        if (service is null)
        {
            return ("You can't update a non-existant service", ResultType.Fail);
        }

        service.Auth = @event.NewAuth;
        await _dbHandler.UpdateAsync(service);
        
        return ("Ok", ResultType.Success);
    }
}