using Alpha.Common.WeatherService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Weather.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class WeatherController(ILogger<WeatherController> logger) : ControllerBase, IWeatherService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherController> _logger = logger;

    [HttpGet]
    [Authorize(Policy = "Weather.Weather.Read")]
    public Task<List<WeatherForecast>> Weather()
    {
        var rng = new Random();
        var ret = Enumerable.Range(1, 100).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToList();

        return Task.FromResult(ret);
    }

}

