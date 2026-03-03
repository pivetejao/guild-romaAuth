using System;
using System.ComponentModel.DataAnnotations;

namespace GuildRomaAuth.Models
{
    public class AppUser
    {

        public int Id { get; set; }

        public string Role { get; set; } = "Membro";

        public string Rank { get; set; } = "D";


        [Required]
        [MaxLength(15)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
    }
}
