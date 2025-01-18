namespace AdvancedServices.Request;

public class UpdateReactionRequest
{
    public Guid ReactionId { get; set; }
    
    public string ExecutionConfig { get; set; }
}