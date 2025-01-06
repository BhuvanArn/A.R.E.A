namespace EventBus.Event;

public class UnsubscribeServiceEvent
{
    public string JwtToken { get; set; }
    
    public string Name { get; set; }

    public Credentials Credentials { get; set; }
}