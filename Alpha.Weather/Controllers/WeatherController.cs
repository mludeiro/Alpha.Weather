using Alpha.Common.WeatherService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Weather.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController(ILogger<WeatherController> logger) : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherController> _logger = logger;

    [HttpGet]
    [Authorize(Policy = "Weather.Weather.Read")]
    public List<WeatherForecast> Get()
    {
        var rng = new Random();
        return Enumerable.Range(1, 100).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToList();
    }
}

