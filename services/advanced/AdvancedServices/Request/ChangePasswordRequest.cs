namespace AdvancedServices.Request;

public class ChangePasswordRequest
{
    public string Password { get; set; }
    
    public string ConfirmPassword { get; set; }
}