namespace EventBus.Event;

public class ChangeUsernameEvent
{
    public string Username { get; set; }
    
    public string JwtToken { get; set; }
}