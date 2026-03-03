using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GuildRomaAuth.Data;
using GuildRomaAuth.DTOs;
using GuildRomaAuth.Models;
using GuildRomaAuth.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GuildRomaAuth.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        //REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailExists = await _context.AppUsers
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExists)
                return BadRequest("Email já registrado");

            var user = new AppUser
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário criado com sucesso" });
        }

        //LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.AppUsers
                .FirstOrDefaultAsync(u =>
                    u.Email == dto.Email || u.Username == dto.Username);

            if (user == null)
                return Unauthorized("Credenciais inválidas");

            var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!valid)
                return Unauthorized("Credenciais inválidas");

            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });
        }

        //PERFIL 
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue("id"); 
            if (!int.TryParse(userId, out var id))
                return Unauthorized();

            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.Rank,
                user.Role,
                user.Username,
                user.Email,
                user.AvatarUrl,
                user.Bio,
                user.CreatedAt
            });
        }

        //ATUALIZAR PERFIL
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue("id"); 
            if (!int.TryParse(userId, out var id))
                return Unauthorized();

            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Username = dto.Username ?? user.Username;
            user.Bio = dto.Bio ?? user.Bio;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Perfil atualizado com sucesso" });
        }

        //UPLOAD AVATAR
        [HttpPost("avatar")]
        [Authorize]
        public async Task<IActionResult> UploadAvatar([FromForm] IFormFile avatar)
        {
            if (avatar == null || avatar.Length == 0)
                return BadRequest("Arquivo inválido");

            var userId = User.FindFirstValue("id"); 
            if (!int.TryParse(userId, out var id))
                return Unauthorized();

            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
                return NotFound();

            var uploadsFolder = Path.Combine("wwwroot", "avatars");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{avatar.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await avatar.CopyToAsync(stream);

            user.AvatarUrl = $"/avatars/{fileName}";
            await _context.SaveChangesAsync();

            return Ok(new { avatarUrl = user.AvatarUrl });
        }
    }
} 