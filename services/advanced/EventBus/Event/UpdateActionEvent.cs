namespace EventBus.Event;

public class UpdateActionEvent
{
    public Guid ActionId { get; set; }
    
    public string TriggerConfig { get; set; }
    
    public string JwtToken { get; set; }
}