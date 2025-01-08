namespace ActionReactionService.AboutParser;

public class ServiceJson
{
    public string Name { get; set; }
    public List<ActionJson> Actions { get; set; } = new();
    public List<ReactionJson> Reactions { get; set; } = new();
}

public class ActionJson
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class ReactionJson
{
    public string Name { get; set; }
    public string Description { get; set; }
}