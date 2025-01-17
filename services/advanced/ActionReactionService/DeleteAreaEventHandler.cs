using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Extension.Socket;
using Action = Database.Entities.Action;

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

            var service = (await _dbHandler.GetAsync<Service>(s => s.Id == @event.ServiceId)).FirstOrDefault();

            if (service is not null)
            {
                await _dbHandler.DeleteAsync(service);
            }

            var actions = (await _dbHandler.GetAsync<Action>(s => s.ServiceId == @event.ServiceId)).ToList();

            if (actions.Count > 0)
            {
                foreach (var action in actions)
                {
                    await _dbHandler.DeleteAsync(action);
                }
            }
            
            var reactions = (await _dbHandler.GetAsync<Reaction>(s => s.ServiceId == @event.ServiceId)).ToList();

            if (reactions.Count > 0)
            {
                foreach (var reaction in reactions)
                {
                    await _dbHandler.DeleteAsync(reaction);
                }
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
        
        if (@event.ActionId is not null)
        {
            var areasToDelete = (await _dbHandler.GetAsync<Area>(a => a.ActionId == @event.ActionId && a.UserId == userId)).ToList();
            if (areasToDelete.Count == 0)
            {
                return ("No areas found for the provided ActionId.", ResultType.Fail);
            }
            
            var actions = (await _dbHandler.GetAsync<Action>(s => s.ServiceId == @event.ServiceId)).ToList();

            if (actions.Count > 0)
            {
                foreach (var action in actions)
                {
                    var reaction = (await _dbHandler.GetAsync<Reaction>(s => s.ServiceId == @event.ServiceId && s.ActionId == action.Id)).FirstOrDefault();

                    if (reaction is not null)
                    {
                        await _dbHandler.DeleteAsync(reaction);
                    }
                    
                    await _dbHandler.DeleteAsync(action);
                }
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
        
        if (@event.ReactionId is not null)
        {
            var areasToDelete = (await _dbHandler.GetAsync<Area>(a => a.ReactionId == @event.ReactionId && a.UserId == userId)).ToList();
            if (areasToDelete.Count == 0)
            {
                return ("No areas found for the provided ReactionId.", ResultType.Fail);
            }
            
            var reaction = (await _dbHandler.GetAsync<Reaction>(s => s.Id == @event.ReactionId)).FirstOrDefault();

            if (reaction is not null)
            {
                await _dbHandler.DeleteAsync(reaction);
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