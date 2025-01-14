namespace AdvancedServices.Request;

public class AddActionRequest
{
    public Guid ServiceId { get; set; }

    public string Name { get; set; }
    
    public string DisplayName { get; set; }
    
    public string TriggerConfig { get; set; }
}