namespace EventBus.Event;

public class GetDiscordTokenEvent
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}