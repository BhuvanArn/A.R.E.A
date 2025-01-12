namespace EventBus.Event;

public class AddReactionEvent
{
    public Guid ServiceId { get; set; }
    
    public Guid ActionId { get; set; }
    
    public string Name { get; set; }
    
    public string ExecutionConfig { get; set; }
    
    public string JwtToken { get; set; }
}