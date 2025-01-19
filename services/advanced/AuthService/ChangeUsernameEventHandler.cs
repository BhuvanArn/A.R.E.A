using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace AuthService;

public class ChangeUsernameEventHandler : IIntegrationEventHandler<ChangeUsernameEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public ChangeUsernameEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(ChangeUsernameEvent @event)
    {
        string id = @event.JwtToken.GetJwtSubClaim();

        if (!Guid.TryParse(id, out Guid userId))
        {
            return ("You are not logged in", ResultType.Fail);
        }
        
        var user = (await _dbHandler.GetAsync<User>(s => s.Id == userId)).FirstOrDefault();

        if (user is null)
        {
            return ("You are not logged in", ResultType.Fail);
        }
        
        if (string.IsNullOrEmpty(user.Password))
        {
            return ("You are not logged in", ResultType.Fail);
        }

        user.Username = @event.Username;
        await _dbHandler.UpdateAsync(user);
        
        return ("Ok", ResultType.Success);
    }
}