namespace EventBus.Event;

public class UpdateReactionEvent
{
    public Guid ReactionId { get; set; }
    
    public string ExecutionConfig { get; set; }
    
    public string JwtToken { get; set; }
}