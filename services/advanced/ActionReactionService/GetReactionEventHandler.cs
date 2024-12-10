using Database.Entities;
using Database.Service;
using EventBus;
using EventBus.Event;
using Extension;

namespace ActionReactionService;

public class GetReactionEventHandler : IIntegrationEventHandler<GetReactionEvent, (List<Reaction>, ResultType)>
{
    private readonly ServiceService _serviceService;
    private readonly ReactionService _reactionService;

    public GetReactionEventHandler(ServiceService serviceService, ReactionService reactionService)
    {
        _serviceService = serviceService;
        _reactionService = reactionService;
    }
    
    public async Task<(List<Reaction>, ResultType)> HandleAsync(GetReactionEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }
        
        var service = (await _serviceService.FindServicesAsync(s => s.UserId == userId && s.Name == @event.ServiceName)).FirstOrDefault();

        if (service == null)
        {
            return (new(), ResultType.Fail);
        }

        var actions = await _reactionService.FindReactionsAsync(s => s.ServiceId == service.Id);
        
        return (actions.ToList(), ResultType.Success);
    }
}