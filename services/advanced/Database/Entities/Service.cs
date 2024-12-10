namespace Database.Entities;

public class Service
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; }
    
    public Guid UserId { get; set; }
    
    public string Auth { get; set; }
    
    public ICollection<Action> Actions { get; set; } = new List<Action>();
    
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    
    public User User { get; set; }
}
