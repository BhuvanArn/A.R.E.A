namespace AdvancedServices.Request;

public class DeleteAreaRequest
{
    public string JwtToken { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid? ActionId { get; set; }
    public Guid? ReactionId { get; set; }
}