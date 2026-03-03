using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GuildRomaAuth.Data;
using GuildRomaAuth.DTOs;
using GuildRomaAuth.Models;

namespace GuildRomaAuth.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController : ControllerBase
{
    private readonly AppDbContext _context;

    public MembersController(AppDbContext context)
    {
        _context = context;
    }

    [Authorize(Roles = "Founder,Admin")]
    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateRole(
    int id,
    [FromBody] UpdateMemberRoleDto dto)
    {
        var member = await _context.GuildMembers.FindAsync(id);
        if (member == null)
            return NotFound();

        if (!Enum.TryParse<GuildRole>(dto.Role, true, out var parsedRole))
            return BadRequest("Role inválida");

        member.Role = parsedRole;
        await _context.SaveChangesAsync();

        return Ok(member);
    }
}
