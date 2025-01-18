using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class UpdateAreaEventHandler : IIntegrationEventHandler<UpdateAreaEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public UpdateAreaEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(UpdateAreaEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not connected", ResultType.Fail);
        }
        
        var area = (await _dbHandler.GetAsync<Area>( s=> s.Id == @event.AreaId && s.UserId == userId)).FirstOrDefault();

        if (area is null)
        {
            return ("You can't update a non-existant area", ResultType.Fail);
        }

        if (!string.IsNullOrEmpty(@event.DisplayName))
        {
            area.DisplayName = @event.DisplayName;
        }

        if (@event.State.HasValue)
        {
            area.State = @event.State.Value;
        }

        await _dbHandler.UpdateAsync(area);
        
        return ("Ok", ResultType.Success);
    }
}