using System.Reactive.Linq;
using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Extension.Socket;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class AddReactionEventHandler : IIntegrationEventHandler<AddReactionEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    private readonly ISocketService _socketService;

    public AddReactionEventHandler(IDatabaseHandler dbHandler, ISocketService socketService)
    {
        _dbHandler = dbHandler;
        _socketService = socketService;
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

        Observable.Timer(TimeSpan.FromMilliseconds(500))
            .Subscribe(_ =>
            {
                try
                {
                    _socketService.OpenSocket();
                    _socketService.SendHandshakeAndNotifyChange();
                    _socketService.CloseSocket();
                }
                catch (Exception)
                {
                    // ignore error
                }
            });
        
        return ("Ok", ResultType.Success);
    }
}