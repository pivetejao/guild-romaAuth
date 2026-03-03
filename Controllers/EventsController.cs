using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildRomaAuth.Data;
using GuildRomaAuth.Models;

namespace GuildRomaAuth.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.GuildEvents
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

            return Ok(events);
        }

        //GET EVENT
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _context.GuildEvents.FindAsync(id);

            if (ev == null)
                return NotFound();

            return Ok(ev);
        }

        // POST

        [Authorize(Roles = "Founder,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GuildEvent model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                return BadRequest("Título é obrigatório");

                var ev = new GuildEvent
                {
                    Title = model.Title,

                    Description = model.Description,

                    IsMandatory = model.IsMandatory,
                    
                    CreatedAt = DateTime.UtcNow
                };
            _context.GuildEvents.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = ev.Id }, ev);
        }

        // UPDATE
        [Authorize(Roles = "Founder,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GuildEvent model)
        {
            var ev = await _context.GuildEvents.FindAsync(id);

            if (ev == null)
                return NotFound();

            ev.Title = model.Title;
            ev.Description = model.Description;
            ev.CreatedAt = model.CreatedAt;

            await _context.SaveChangesAsync();
            return Ok(ev);
        }

        // DELETE
        [Authorize(Roles = "Founder,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.GuildEvents.FindAsync(id);

            if (ev == null)
                return NotFound(new { message = "Evento não encontrado." });

            _context.GuildEvents.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}
