using Microsoft.AspNetCore.Mvc;
using GuildRomaAuth.Data;
using GuildRomaAuth.Models;

namespace GuildRomaAuth.Controllers
{
    [ApiController]
    [Route("api/guild")]
    public class GuildController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GuildController(AppDbContext context)
        {
            _context = context;
        }

       
        // DASHBOARD (GET)
        
        [HttpGet("dashboard")]
        public IActionResult GetDashboard()
        {
            var settings = _context.GuildSettings.FirstOrDefault();

            if (settings == null)
            {
                return Ok(new
                {
                    guildStatus = "Em Paz",
                    taxRate = 15,
                    totalFame = 0,
                    members = new List<object>()
                });
            }

            var members = _context.AppUsers
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Role,
                    u.Rank
                })
                .ToList();

            return Ok(new
            {
                guildStatus = settings.GuildStatus,
                taxRate = settings.TaxRate,
                totalFame = settings.TotalFame,
                members
            });
        }
    }
}
