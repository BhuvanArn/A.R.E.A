using Database.Entities;
using Database.Service;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class SubscribeServiceEventHandler : IIntegrationEventHandler<SubscribeServiceEvent, (List<Service>, ResultType)>
{
    private readonly ServiceService _serviceService;

    public SubscribeServiceEventHandler(ServiceService serviceService)
    {
        _serviceService = serviceService;
    }
    
    public async Task<(List<Service>, ResultType)> HandleAsync(SubscribeServiceEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var existingService = await _serviceService.FindServicesAsync(s => s.Name == @event.Name && s.UserId == userId);
        
        var service = new Service
        {
            Name = @event.Name,
            UserId = userId,
            Auth = System.Text.Json.JsonSerializer.Serialize(@event.Credentials)
        };
        
        await _serviceService.CreateServiceAsync(service);
        return (existingService.ToList(), ResultType.Success);
    }
}