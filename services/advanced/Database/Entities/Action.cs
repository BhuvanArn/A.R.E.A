namespace Database.Entities;

public class Action
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid ServiceId { get; set; }
    
    public Service Service { get; set; }
    
    public string Name { get; set; }
    
    public string TriggerConfig { get; set; }
}
