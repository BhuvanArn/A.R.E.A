using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace AuthService;

public class ChangePasswordEventHandler : IIntegrationEventHandler<ChangePasswordEvent, (string, ResultType)>
{
    private readonly IDatabaseHandler _dbHandler;

    public ChangePasswordEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(ChangePasswordEvent @event)
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

        if (!string.Equals(@event.ConfirmPassword, @event.Password))
        {
            return ("Passwords do not match", ResultType.Fail);
        }
        
        string password = @event.Password.HashPassword(out string salt);

        user.Password = password;
        user.Salt = salt;
        
        await _dbHandler.UpdateAsync(user);
        
        return ("Ok", ResultType.Success);
    }
}