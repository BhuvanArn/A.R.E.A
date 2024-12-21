using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class GetReactionEventHandler : IIntegrationEventHandler<GetReactionEvent, (List<Reaction>, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public GetReactionEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(List<Reaction>, ResultType)> HandleAsync(GetReactionEvent @event)
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

        var reactions = await _dbHandler.GetAsync<Reaction>(s => s.ServiceId == service.Id);
        
        return (reactions.ToList(), ResultType.Success);
    }
}