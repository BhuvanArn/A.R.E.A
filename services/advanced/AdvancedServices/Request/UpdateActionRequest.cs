namespace AdvancedServices.Request;

public class UpdateActionRequest
{
    public Guid ActionId { get; set; }
    
    public string TriggerConfig { get; set; }
}