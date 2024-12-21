namespace EventBus.Event;

public class SubscribeServiceEvent
{
    public string JwtToken { get; set; }
    
    public string Name { get; set; }

    public Credentials Credentials { get; set; }
}

public class Credentials
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}