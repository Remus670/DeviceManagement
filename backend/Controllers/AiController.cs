using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AiController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("generate-description")]
        public async Task<IActionResult> GenerateDescription([FromBody] DeviceDescriptionRequest request)
        {
           
           var apiKey =  "INSERT API KEY HERE" ;
Console.WriteLine($"API Key used: {apiKey}");
            var prompt = $"Generate a concise, human-readable description for a device with these specs: " +
                        $"Name: {request.Name}, Manufacturer: {request.Manufacturer}, " +
                        $"Type: {request.Type}, OS: {request.OperatingSystem}, " +
                        $"RAM: {request.RamAmount}, Processor: {request.Processor}. " +
                        $"Keep it under 2 sentences, professional and informative.";

            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[] { new { text = prompt } }
                    }
                }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

       var httpRequest = new HttpRequestMessage(
    HttpMethod.Post,
    "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent"
);
            httpRequest.Headers.Add("x-goog-api-key", apiKey);
            httpRequest.Content = content;

            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(httpRequest);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest($"Gemini API error: {responseBody}");

            var result = JsonSerializer.Deserialize<GeminiResponse>(responseBody);
            var description = result?.candidates?[0]?.content?.parts?[0]?.text ?? "No description generated";

            return Ok(new { description });
        }
    }

    public class DeviceDescriptionRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public string RamAmount { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
    }

    public class GeminiResponse
    {
        public GeminiCandidate[]? candidates { get; set; }
    }

    public class GeminiCandidate
    {
        public GeminiContent? content { get; set; }
    }

    public class GeminiContent
    {
        public GeminiPart[]? parts { get; set; }
    }

    public class GeminiPart
    {
        public string? text { get; set; }
    }
}