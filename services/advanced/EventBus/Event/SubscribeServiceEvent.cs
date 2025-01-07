namespace EventBus.Event;

public class SubscribeServiceEvent
{
    public string JwtToken { get; set; }
    
    public string Name { get; set; }
    
    public object Auth { get; set; }
}