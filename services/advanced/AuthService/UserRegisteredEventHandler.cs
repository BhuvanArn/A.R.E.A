using Database;
using Database.Entities;
using EventBus;
using EventBus.Event;
using Extension;

namespace AuthService;

public class UserRegisteredEventHandler : IIntegrationEventHandler<UserRegisteredEvent, (string, ResultType)>
{
    private IDatabaseHandler _dbHandler;

    public UserRegisteredEventHandler(IDatabaseHandler dbHandler)
    {
        _dbHandler = dbHandler;
    }
    
    public async Task<(string, ResultType)> HandleAsync(UserRegisteredEvent @event)
    {
        if (!string.Equals(@event.Password, @event.ConfirmedPassword))
        {
            return ("Password are not the same", ResultType.Fail);
        }

        var existingUser = (await _dbHandler.GetAsync<User>(s => s.Email == @event.Email)).FirstOrDefault();

        if (existingUser is not null)
        {
            return ($"User with email {@event.Email} already exists.", ResultType.Fail);
        }

        string password = @event.Password.HashPassword(out string salt);

        User user = new()
        {
            Username = @event.Username,
            Email = @event.Email,
            Password = password,
            Salt = salt
        };
        
        await _dbHandler.AddAsync(user);
        return ("OK", ResultType.Success);
    }
}