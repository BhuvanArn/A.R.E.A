namespace EventBus.Event;

public class GetActionsReactionsEvent
{
    public string ServiceName { get; set; }
    
    public string JwtToken { get; set; }
}