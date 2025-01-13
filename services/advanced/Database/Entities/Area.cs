namespace Database.Entities;

public class Area
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedDate { get; set; }
    
    public string DisplayName { get; set; }
    
    public AreaState State { get; set; } 
    
    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
    
    public Guid? ActionId { get; set; }
    public Action? Action { get; set; }
    
    public Guid? ReactionId { get; set; }
    public Reaction? Reaction { get; set; }
}

public enum AreaState : byte
{
    Active = 1,
    Inactive = 2
}