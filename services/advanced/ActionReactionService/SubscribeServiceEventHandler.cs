using ActionReactionService.AboutParser;
using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ActionReactionService;

public class SubscribeServiceEventHandler : IIntegrationEventHandler<SubscribeServiceEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    private readonly IAboutParserService _aboutParserService;

    public SubscribeServiceEventHandler(IDatabaseHandler dbHandler, IAboutParserService aboutParserService)
    {
        _dbHandler = dbHandler;
        _aboutParserService = aboutParserService;
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
        
        var existingService = (await _dbHandler.GetAsync<Service>(s => s.Name == @event.Name && s.UserId == userId)).FirstOrDefault();

        if (existingService is not null)
        {
            return ($"You already subscribed to {@event.Name}", ResultType.Fail);
        }
        
        var service = new Service
        {
            Name = @event.Name,
            UserId = userId,
            Auth = JsonConvert.SerializeObject(@event.Auth ?? "")
        };
        
        await _dbHandler.AddAsync(service);
        return ($"You subscribed to {@event.Name}", ResultType.Success);
    }
}