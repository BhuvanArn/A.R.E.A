using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Extension.Socket;

namespace ActionReactionService;

public class DeleteAreaEventHandler : IIntegrationEventHandler<DeleteAreaEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    private readonly ISocketService _socketService;

    public DeleteAreaEventHandler(IDatabaseHandler dbHandler, ISocketService socketService)
    {
        _dbHandler = dbHandler;
        _socketService = socketService;
    }
    
    public async Task<(string, ResultType)> HandleAsync(DeleteAreaEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not connected", ResultType.Fail);
        }
        
        if (@event.ServiceId is not null)
        {
            var areasToDelete = (await _dbHandler.GetAsync<Area>(a => a.ServiceId == @event.ServiceId && a.UserId == userId)).ToList();
            if (areasToDelete.Count == 0)
            {
                return ("No areas found for the provided ServiceId.", ResultType.Fail);
            }

            foreach (var area in areasToDelete)
            {
                await _dbHandler.DeleteAsync(area);
            }
            
            _socketService.OpenSocket();
            _socketService.SendHandshake();
            _socketService.NotifyChange();
            _socketService.CloseSocket();

            return ($"Deleted {areasToDelete.Count} areas for ServiceId {@event.ServiceId}.", ResultType.Success);
        }
        
        if (@event.ActionId != null)
        {
            var areasToDelete = (await _dbHandler.GetAsync<Area>(a => a.ActionId == @event.ActionId && a.UserId == userId)).ToList();
            if (areasToDelete.Count == 0)
            {
                return ("No areas found for the provided ActionId.", ResultType.Fail);
            }

            foreach (var area in areasToDelete)
            {
                await _dbHandler.DeleteAsync(area);
            }
            
            _socketService.OpenSocket();
            _socketService.SendHandshake();
            _socketService.NotifyChange();
            _socketService.CloseSocket();

            return ($"Deleted {areasToDelete.Count} areas for ActionId {@event.ActionId}.", ResultType.Success);
        }
        
        if (@event.ReactionId != null)
        {
            var areasToDelete = (await _dbHandler.GetAsync<Area>(a => a.ReactionId == @event.ReactionId && a.UserId == userId)).ToList();
            if (areasToDelete.Count == 0)
            {
                return ("No areas found for the provided ReactionId.", ResultType.Fail);
            }

            foreach (var area in areasToDelete)
            {
                await _dbHandler.DeleteAsync(area);
            }

            _socketService.OpenSocket();
            _socketService.SendHandshake();
            _socketService.NotifyChange();
            _socketService.CloseSocket();
            
            return ($"Deleted {areasToDelete.Count} areas for ReactionId {@event.ReactionId}.", ResultType.Success);
        }
        
        return ("No valid identifier provided for deletion.", ResultType.Fail);
    }
}