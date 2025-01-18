using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Extension.Socket;

namespace ActionReactionService;

public class AddActionEventHandler : IIntegrationEventHandler<AddActionEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    private readonly ISocketService _socketService;

    public AddActionEventHandler(IDatabaseHandler dbHandler, ISocketService socketService)
    {
        _dbHandler = dbHandler;
        _socketService = socketService;
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
        
        var addedAction = await _dbHandler.AddAsync(action);
        
        var area = new Area
        {
            ServiceId = service.Id,
            DisplayName = @event.DisplayName,
            CreatedDate = DateTime.UtcNow,
            ActionId = addedAction.Id,
            State = AreaState.Active,
            UserId = userId
        };

        await _dbHandler.AddAsync(area);

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

        return ($"{addedAction.Id}", ResultType.Success);
    }
}