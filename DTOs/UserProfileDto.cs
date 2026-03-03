using System.ComponentModel.DataAnnotations;

namespace GuildRomaAuth.DTOs;

public class UserProfileDto
{

    public string Username { get; set; }
    
    public string Email { get; set; }

    public string AvatarUrl { get; set; }

}


