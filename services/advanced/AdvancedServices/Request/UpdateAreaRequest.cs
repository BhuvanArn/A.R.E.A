namespace AdvancedServices.Request;

public class UpdateAreaRequest
{
    public Guid AreaId { get; set; }
    
    public string? DisplayName { get; set; }
    
    public byte? State { get; set; }
}