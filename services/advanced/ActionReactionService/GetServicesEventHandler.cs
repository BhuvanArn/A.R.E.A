using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class GetServicesEventHandler : IIntegrationEventHandler<GetServiceEvent, (List<Service>, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public GetServicesEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(List<Service>, ResultType)> HandleAsync(GetServiceEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var services = await _dbHandler.GetAsync<Service>(s => s.UserId == userId);
        
        return (services.ToList(), ResultType.Success);
    }
}