using EventBus.Event;

namespace AdvancedServices.Request;

public class SubscribeServiceRequest
{
    public string Name { get; set; }

    public Credentials Credentials { get; set; }
}