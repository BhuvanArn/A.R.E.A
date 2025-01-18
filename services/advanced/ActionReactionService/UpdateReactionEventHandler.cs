using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class UpdateReactionEventHandler : IIntegrationEventHandler<UpdateReactionEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public UpdateReactionEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(UpdateReactionEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not connected", ResultType.Fail);
        }

        var reaction = (await _dbHandler.GetAsync<Reaction>(s => s.Id == @event.ReactionId)).FirstOrDefault();

        if (reaction is null)
        {
            return ("You can't update a non-existant reaction", ResultType.Fail);
        }
        
        var service = (await _dbHandler.GetAsync<Service>(s => s.Id == reaction.ServiceId && s.UserId == userId)).FirstOrDefault();

        if (service is null)
        {
            return ("You can't update a non-existant reaction", ResultType.Fail);
        }

        reaction.ExecutionConfig = @event.ExecutionConfig;
        await _dbHandler.UpdateAsync(reaction);
        
        return ("Ok", ResultType.Success);
    }
}