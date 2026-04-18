using DeviceManagementApi.Data;
using DeviceManagementApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DeviceManagementApi.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Search query cannot be empty");

            var tokens = NormalizeQuery(q);

            if (tokens.Length == 0)
                return BadRequest("Search query contains no valid terms");

            var devices = await _context.Devices
                .Include(d => d.User)
                .ToListAsync();

            var scored = devices
                .Select(d => new { Device = d, Score = CalculateScore(d, tokens) })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Select(x => x.Device)
                .ToList();

            return Ok(scored);
        }

        private static string[] NormalizeQuery(string query)
        {
            var lower = query.ToLowerInvariant();
            var clean = Regex.Replace(lower, @"[\s\-_.,;:!?]", "");
            var tokens = new[] { clean };
            
            var withSpaces = query.ToLowerInvariant().Trim();
            var spacedTokens = Regex.Replace(withSpaces, @"[^\w\s]", " ")
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            return tokens.Concat(spacedTokens).Distinct().ToArray();
        }

        private static int CalculateScore(Device device, string[] tokens)
        {
            int score = 0;

            var name = Normalize(device.Name);
            var manufacturer = Normalize(device.Manufacturer);
            var processor = Normalize(device.Processor);
            var ram = Normalize(device.RamAmount);

            foreach (var token in tokens)
            {
                if (name.Contains(token)) score += 4;
                if (manufacturer.Contains(token)) score += 3;
                if (processor.Contains(token)) score += 2;
                if (ram.Contains(token)) score += 1;
            }

            return score;
        }

        private static string Normalize(string value)
        {
            var lower = value.ToLowerInvariant();
            return Regex.Replace(lower, @"[\s\-_.,;:!?]", "");
        }
    }
}