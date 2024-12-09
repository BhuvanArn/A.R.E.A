namespace Database.Entities;

public class Reaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid ServiceId { get; set; }
    
    public Service Service { get; set; }
    
    public string Name { get; set; }
    
    public string ExecutionConfig { get; set; }
}
