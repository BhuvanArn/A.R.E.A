using Database.Entities;
using Database.Service;
using EventBus;
using EventBus.Event;
using Extension;

namespace RegisterService;

public class UserRegisteredEventHandler : IIntegrationEventHandler<UserRegisteredEvent, (string, ResultType)>
{
    private UserService _userService;

    public UserRegisteredEventHandler(UserService userService)
    {
        _userService = userService;
    }
    
    public async Task<(string, ResultType)> HandleAsync(UserRegisteredEvent @event)
    {
        if (!string.Equals(@event.Password, @event.ConfirmedPassword))
        {
            return ("Password are not the same", ResultType.Fail);
        }

        var existingUser = (await _userService.FindUsersAsync(s => s.Email == @event.Email)).FirstOrDefault();

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
        
        await _userService.CreateUserAsync(user);
        return ("OK", ResultType.Success);
    }
}