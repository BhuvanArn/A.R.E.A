namespace EventBus.Event;

public class UserResetPasswordEvent
{
    public string JwtToken { get; set; }
    
    public string Password { get; set; }
    
    public string NewPassword { get; set; }
}