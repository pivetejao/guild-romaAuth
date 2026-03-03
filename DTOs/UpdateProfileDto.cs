using System.ComponentModel.DataAnnotations;

namespace GuildRomaAuth.DTOs;

public class UpdateProfileDto
{
    public string Username { get; set; }
    public string Bio { get; set; }

}