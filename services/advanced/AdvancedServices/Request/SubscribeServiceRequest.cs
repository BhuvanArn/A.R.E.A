using EventBus.Event;

namespace AdvancedServices.Request;

public class SubscribeServiceRequest
{
    public string Name { get; set; }
    
    public object Auth { get; set; }
}