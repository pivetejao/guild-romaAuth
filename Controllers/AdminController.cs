using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GuildRomaAuth.Data;
using GuildRomaAuth.Models;

namespace GuildRomaAuth.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

      
        // UPDATE GUILD SETTINGS
        
        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] GuildSettings model)
        {
            var settings = _context.GuildSettings.FirstOrDefault();

            if (settings == null)
            {
                _context.GuildSettings.Add(model);
            }
            else
            {
                settings.GuildStatus = model.GuildStatus;
                settings.TaxRate = model.TaxRate;
                settings.TotalFame = model.TotalFame;
                settings.UpdateAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok(model);
        }

        
        // UPDATE  ROLE + RANK
        
        [HttpPut("member/{id}")]
        public async Task<IActionResult> UpdateMember(
            int id,
            [FromQuery] string role,
            [FromQuery] string rank)
        {
            var user = _context.AppUsers.FirstOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            user.Role = role;
            user.Rank = rank;

            await _context.SaveChangesAsync();
            return Ok(new { forceLogout = true });
        }
    }
}
