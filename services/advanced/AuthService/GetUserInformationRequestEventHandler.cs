using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace AuthService;

public class GetUserInformationRequestEventHandler : IIntegrationEventHandler<GetUserInformationEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public GetUserInformationRequestEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(GetUserInformationEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not connected", ResultType.Fail);
        }
        
        var user = (await _dbHandler.GetAsync<User>(s => s.Id == userId)).FirstOrDefault();

        return user == null ? ("You are not connected", ResultType.Fail) : ($"{user.Email};{user.Username}", ResultType.Success);
    }
}