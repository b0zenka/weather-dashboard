using Microsoft.Extensions.Caching.Memory;
using WeatherProxy.Models.Dtos;

namespace WeatherProxy.Services;

public class WeatherService : IWeatherService
{
    private readonly IOpenMeteoClient _client;
    private readonly IMemoryCache _cache;

    public WeatherService(IOpenMeteoClient openMeteoClient, IMemoryCache cache)
    {
        _client = openMeteoClient;
        _cache = cache;
    }

    public async Task<WeatherDto> GetWeatherAsync(string city, int days)
    {
        var key = $"{city}:{days}";
        if (_cache.TryGetValue(key, out WeatherDto cached))
            return cached;

        var (latitude, longitude) = await _client.GeocodeAsync(city);
        var forecast = await _client.GetForecastAsync(latitude, longitude, days);

        if (forecast == null)
            throw new Exception("Forecast unavailable");

        var dto = new WeatherDto
        {
            City = city,
            Coordinates = new CoordinatesDto
            {
                Latitude = latitude,
                Longitude = longitude
            },
            Timezone = forecast.timezone,
            Hourly = forecast.hourly.time
                .Select((t, i) => new HourPointDto
                {
                    Time = t,
                    Temperature = forecast.hourly.temperature_2m[i],
                    Precipitation = forecast.hourly.precipitation[i],
                    Wind = forecast.hourly.wind_speed_10m[i],
                }).ToList(),
            Daily = forecast.daily.time
                .Select((t, i) => new DailyPointDto
                {
                    Date = t,
                    TemperatureMin = forecast.daily.temperature_2m_min[i],
                    TemperatureMax = forecast.daily.temperature_2m_max[i],
                }).ToList()
        };

        _cache.Set(key, dto, TimeSpan.FromMinutes(30));

        return dto;
    }
}
