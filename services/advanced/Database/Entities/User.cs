namespace Database.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Salt { get; set; }
    
    public ICollection<Service> Services { get; set; } = new List<Service>();
}