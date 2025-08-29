using Microsoft.AspNetCore.Mvc;
using WeatherProxy.Services;

namespace WeatherProxy.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly WeatherService _weatherService;
    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{city}")]
    public async Task<IActionResult> GetWeather(string city, [FromQuery] int days = 3)
    {
        try
        {
            var weather = await _weatherService.GetWeatherAsync(city, days);
            return Ok(weather);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"City '{city}' not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(502, new { message = ex.Message });
        }
    }
}
