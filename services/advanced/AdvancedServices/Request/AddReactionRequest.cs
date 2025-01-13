namespace AdvancedServices.Request;

public class AddReactionRequest
{
    public Guid ServiceId { get; set; }
    
    public Guid ActionId { get; set; }
    
    public string Name { get; set; }
    
    public string ExecutionConfig { get; set; }
}