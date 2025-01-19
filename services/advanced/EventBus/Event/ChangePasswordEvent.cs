namespace EventBus.Event;

public class ChangePasswordEvent
{
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
    
    public string JwtToken { get; set; }
}