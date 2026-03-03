using System.ComponentModel.DataAnnotations;

namespace GuildRomaAuth.DTOs;

public class LoginDto
{
    
    public string Username { get; set; } = string.Empty;

    
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string CreatedAt { get; set; } = string.Empty;
}
