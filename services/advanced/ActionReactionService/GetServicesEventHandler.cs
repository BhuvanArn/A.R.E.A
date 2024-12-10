using Database.Entities;
using Database.Service;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class GetServicesEventHandler : IIntegrationEventHandler<GetServiceEvent, (List<Service>, ResultType)>
{
    private readonly ServiceService _serviceService;

    public GetServicesEventHandler(ServiceService serviceService)
    {
        _serviceService = serviceService;
    }
    
    public async Task<(List<Service>, ResultType)> HandleAsync(GetServiceEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var services = await _serviceService.FindServicesAsync(s => s.UserId == userId);
        
        return (services.ToList(), ResultType.Success);
    }
}