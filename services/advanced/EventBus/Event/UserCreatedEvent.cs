namespace EventBus.Event;

public class UserCreatedEvent
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}