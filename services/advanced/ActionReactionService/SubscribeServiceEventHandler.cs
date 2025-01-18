using ActionReactionService.AboutParser;
using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Extension.Socket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ActionReactionService;

public class SubscribeServiceEventHandler : IIntegrationEventHandler<SubscribeServiceEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    private readonly IAboutParserService _aboutParserService;
    private readonly ISocketService _socketService;

    public SubscribeServiceEventHandler(IDatabaseHandler dbHandler, IAboutParserService aboutParserService, ISocketService socketService)
    {
        _dbHandler = dbHandler;
        _aboutParserService = aboutParserService;
        _socketService = socketService;
    }
    
    public async Task<(string, ResultType)> HandleAsync(SubscribeServiceEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();
        
        var services = _aboutParserService.GetServices();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not connected", ResultType.Fail);
        }

        if (services.All(s => s.Name != @event.Name))
        {
            var availableServices = string.Join(", ", services.Select(s => s.Name));
            return ($"The service {@event.Name} doesnt exist, avalaible ones: {availableServices}", ResultType.Fail);
        }
        
        var service = new Service
        {
            Name = @event.Name,
            UserId = userId,
            Auth = JsonConvert.SerializeObject(@event.Auth ?? "")
        };
        
        await _dbHandler.AddAsync(service);
        
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
        
        return ($"You subscribed to {@event.Name}", ResultType.Success);
    }
}