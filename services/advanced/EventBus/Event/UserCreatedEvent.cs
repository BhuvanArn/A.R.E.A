namespace EventBus.Event;

public class UserCreatedEvent
{
    public string Username { get; set; }
    
    public string Email { get; set; }
}