using Microsoft.EntityFrameworkCore;
using GuildRomaAuth.Models;

namespace GuildRomaAuth.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<GuildSettings> GuildSettings { get; set; }

    public DbSet<GuildEvent> GuildEvents { get; set; }

    public DbSet<GuildMember> GuildMembers { get; set; }
}
