using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace GuildRomaAuth.Controllers
{
    [ApiController]
    [Route("api/albion")]
    public class AlbionController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://gameinfo.albiononline.com/api/gameinfo";

        public AlbionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Add(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64)"
            );
        }

        [HttpGet("player/{nickname}")]
        public async Task<IActionResult> GetPlayerStats(string nickname)
        {
            var searchResponse =
                await _httpClient.GetFromJsonAsync<SearchResponse>(
                    $"{BaseUrl}/search?q={nickname}"
                );

            var player = searchResponse?.Players?.FirstOrDefault();
            if (player == null)
                return NotFound("Jogador não encontrado");

            var details =
                await _httpClient.GetFromJsonAsync<PlayerDetails>(
                    $"{BaseUrl}/players/{player.Id}"
                );

            return Ok(details);
        }
    }

    // MODELOS
    public class SearchResponse
    {
        public List<PlayerSearchItem>? Players { get; set; }
    }

    public class PlayerSearchItem
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }

    public class PlayerDetails
    {
        public string? Name { get; set; }
        public long KillFame { get; set; }
        public string? GuildName { get; set; }
        public LifetimeStats? LifetimeStatistics { get; set; }
    }

    public class LifetimeStats
    {
        public object? PvE { get; set; }
    }
}
