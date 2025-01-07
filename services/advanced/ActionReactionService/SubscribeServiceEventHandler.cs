using ActionReactionService.AboutParser;
using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Newtonsoft.Json;

namespace ActionReactionService;

public class SubscribeServiceEventHandler : IIntegrationEventHandler<SubscribeServiceEvent, (List<Service>, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;
    private readonly IAboutParserService _aboutParserService;

    public SubscribeServiceEventHandler(IDatabaseHandler dbHandler, IAboutParserService aboutParserService)
    {
        _dbHandler = dbHandler;
        _aboutParserService = aboutParserService;
    }
    
    public async Task<(List<Service>, ResultType)> HandleAsync(SubscribeServiceEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var services = _aboutParserService.GetServices();

        if (services.All(s => s.Name != @event.Name))
        {
            return (new(), ResultType.Fail);
        }
        
        var existingService = await _dbHandler.GetAsync<Service>(s => s.Name == @event.Name && s.UserId == userId);
        
        var service = new Service
        {
            Name = @event.Name,
            UserId = userId,
            Auth = JsonConvert.SerializeObject(@event.Auth)
        };
        
        await _dbHandler.AddAsync(service);
        return (existingService.ToList(), ResultType.Success);
    }
}