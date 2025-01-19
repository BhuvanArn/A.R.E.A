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
            var servicesToDelete = (await _dbHandler.GetAsync<Service>(a => a.Id == @event.ServiceId && a.UserId == userId)).ToList();

            foreach (var service in servicesToDelete)
            {
                var area = (await _dbHandler.GetAsync<Area>(s => s.ServiceId == service.Id)).FirstOrDefault();
                
                if (area is not null)
                {
                    await _dbHandler.DeleteAsync(area);
                }

                var actions = (await _dbHandler.GetAsync<Action>(s => s.ServiceId == service.Id)).ToList();

                if (actions.Count > 0)
                {
                    foreach (var action in actions)
                    {
                        var reactions = (await _dbHandler.GetAsync<Reaction>(s => s.ActionId == action.Id)).ToList();

                        foreach (var reaction in reactions)
                        {
                            await _dbHandler.DeleteAsync(reaction);
                        }
                        
                        await _dbHandler.DeleteAsync(action);
                    }
                }

                var toDelete = servicesToDelete.FirstOrDefault();

                if (toDelete is not null)
                {
                    await _dbHandler.DeleteAsync(toDelete);
                }
            }
            
            try
            {
                _socketService.OpenSocket();
                _socketService.SendHandshakeAndNotifyChange();
                _socketService.CloseSocket();
            }
            catch (Exception)
            {
                // ignored
            }

            return ($"Deleted {servicesToDelete.Count} areas for ServiceId {@event.ServiceId}.", ResultType.Success);
        }
        
        if (@event.ActionId is not null)
        {
            var actionToDelete = (await _dbHandler.GetAsync<Action>(a => a.Id == @event.ActionId)).FirstOrDefault();

            if (actionToDelete is null)
            {
                return ("", ResultType.Fail);
            }
            
            var area = (await _dbHandler.GetAsync<Area>(s => s.ActionId == actionToDelete.Id)).FirstOrDefault();

            if (area is not null)
            {
                await _dbHandler.DeleteAsync(area);
            }
            
            var service = (await _dbHandler.GetAsync<Service>(s => s.Id == actionToDelete.ServiceId && s.UserId == userId)).FirstOrDefault();

            if (service is null)
            {
                return ("", ResultType.Fail);
            }

            var reactions = (await _dbHandler.GetAsync<Reaction>(s => s.ActionId == actionToDelete.Id)).ToList();

            foreach (var reaction in reactions)
            {
                await _dbHandler.DeleteAsync(reaction);
            }

            await _dbHandler.DeleteAsync(actionToDelete);
            
            try
            {
                _socketService.OpenSocket();
                _socketService.SendHandshakeAndNotifyChange();
                _socketService.CloseSocket();
            }
            catch (Exception)
            {
                // ignored
            }

            return ($"Delete ActionId {@event.ActionId}.", ResultType.Success);
        }
        
        if (@event.ReactionId is not null)
        {
            var reactionToDelete = (await _dbHandler.GetAsync<Reaction>(a => a.Id == @event.ReactionId)).FirstOrDefault();

            if (reactionToDelete is null)
            {
                return ("", ResultType.Fail);
            }
            
            var service = (await _dbHandler.GetAsync<Service>(s => s.Id == reactionToDelete.ServiceId && s.UserId == userId)).FirstOrDefault();

            if (service is null)
            {
                return ("", ResultType.Fail);
            }
            
            var area = (await _dbHandler.GetAsync<Area>(s => s.ReactionId == reactionToDelete.Id)).FirstOrDefault();

            if (area is not null)
            {
                area.ReactionId = null;
                await _dbHandler.UpdateAsync(area);
            }
            
            await _dbHandler.DeleteAsync(reactionToDelete);

            try
            {
                _socketService.OpenSocket();
                _socketService.SendHandshakeAndNotifyChange();
                _socketService.CloseSocket();
            }
            catch (Exception)
            {
                // ignored
            }
            
            return ($"Deleted ReactionId {@event.ReactionId}.", ResultType.Success);
        }
        
        return ("No valid identifier provided for deletion.", ResultType.Fail);
    }
}