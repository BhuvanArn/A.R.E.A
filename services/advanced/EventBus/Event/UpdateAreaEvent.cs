using Database.Entities;

namespace EventBus.Event;

public class UpdateAreaEvent
{
    public Guid AreaId { get; set; }
    
    public string? DisplayName { get; set; }
    
    public AreaState? State { get; set; }
    
    public string JwtToken { get; set; }
}