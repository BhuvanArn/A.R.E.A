namespace EventBus.Event;

public class GetServiceEvent
{
    public string JwtToken { get; set; }
    
    public bool GetArea { get; set; }
}