namespace GuildRomaAuth.Models;

public enum GuildEventType
{
    CTA,
    Training
}

public class GuildEvent
{
    public int Id { get; set; }
    
    public string Title { get; set; } = "";
    
    public string Description { get; set; } = "";
    
    public bool IsMandatory { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
