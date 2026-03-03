using System;

namespace GuildRomaAuth.Models;

public class GuildMember
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;
    
    public string Username { get; set; } = string.Empty;

    public long Fame { get; set; }

    public GuildRole Role { get; set; } = GuildRole.Member;

    public GuildRank Rank { get; set; } = GuildRank.D;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
