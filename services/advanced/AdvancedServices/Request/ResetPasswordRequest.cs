namespace AdvancedServices.Request;

public class ResetPasswordRequest
{
    public string JwtToken { get; set; }
    
    public string Password { get; set; }
    
    public string NewPassword { get; set; }
}