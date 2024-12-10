namespace EventBus.Event;

public class GetActionEvent
{
    public string ServiceName { get; set; }
    
    public string JwtToken { get; set; }
}