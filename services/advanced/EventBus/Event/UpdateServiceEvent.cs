namespace EventBus.Event;

public class UpdateServiceEvent
{
    public string NewAuth { get; set; }
    
    public string JwtToken { get; set; }
    
    public Guid ServiceId { get; set; }
}