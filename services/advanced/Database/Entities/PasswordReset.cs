namespace Database.Entities;

public class PasswordReset
{
    public int Id { get; set; }
    public string UserEmail { get; set; }
    public string ResetToken { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}