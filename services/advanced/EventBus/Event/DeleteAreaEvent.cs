namespace EventBus.Event;

public class DeleteAreaEvent
{
    public string JwtToken { get; set; }
    public Guid? ServiceId { get; set; }
    
    public Guid? ActionId { get; set; }
    
    public Guid? ReactionId { get; set; }
}