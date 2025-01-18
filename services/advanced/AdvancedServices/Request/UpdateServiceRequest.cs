namespace AdvancedServices.Request;

public class UpdateServiceRequest
{
    public string NewAuth { get; set; }
    
    public Guid ServiceId { get; set; }
}