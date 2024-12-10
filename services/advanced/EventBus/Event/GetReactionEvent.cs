namespace EventBus.Event;

public class GetReactionEvent
{
    public string ServiceName { get; set; }
    
    public string JwtToken { get; set; }
}