using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;
using Action = Database.Entities.Action;

namespace ActionReactionService;

public class GetServicesEventHandler : IIntegrationEventHandler<GetServiceEvent, (object, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public GetServicesEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }

    public async Task<(object, ResultType)> HandleAsync(GetServiceEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return (new(), ResultType.Fail);
        }

        var services = (await _dbHandler.GetAsync<Service>(s => s.UserId == userId)).ToList();

        if (!@event.GetArea)
        {
            return (services, ResultType.Success);
        }

        var areasWithDetails = new List<Area>();

        foreach (var service in services)
        {
            var area = (await _dbHandler.GetAsync<Area>(s =>
                s.ServiceId == service.Id && !string.IsNullOrEmpty(s.DisplayName))).FirstOrDefault();

            if (area == null)
            {
                continue;
            }

            area.Action = area.ActionId != null
                ? (await _dbHandler.GetAsync<Action>(a => a.Id == area.ActionId)).FirstOrDefault()
                : null;

            area.Reaction = area.ReactionId != null
                ? (await _dbHandler.GetAsync<Reaction>(r => r.Id == area.ReactionId)).FirstOrDefault()
                : null;

            areasWithDetails.Add(area);
        }

        return (areasWithDetails, ResultType.Success);
    }

    private class AreaInfo
    {
        public Area Area { get; set; }
    }
}