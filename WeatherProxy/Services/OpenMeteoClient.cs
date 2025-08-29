using WeatherProxy.Models.OpenMeteoResponses;
using static System.Net.WebRequestMethods;

namespace WeatherProxy.Services;

public class OpenMeteoClient
{
    private readonly HttpClient _httpClient;

    public OpenMeteoClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("openmeteo");
    }

    public async Task<(double latitude, double longitude)> GeocodeAsync(string city)
    {
        var url = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(city)}&count=1&language=pl";
        var response = await _httpClient.GetFromJsonAsync<GeocodingResponse>(url);
        var result = response?.results?.FirstOrDefault();

        if (result == null)
            throw new KeyNotFoundException("City not found");
        
        return (result.latitude, result.longitude);
    }

    public async Task<ForecastResponse?> GetForecastAsync(double latitude, double longitude, int days)
    {
        var url = $"https://api.open-meteo.com/v1/forecast" +
                  $"?latitude={latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                  $"&longitude={longitude.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                  $"&hourly=temperature_2m,precipitation,wind_speed_10m" +
                  $"&daily=temperature_2m_max,temperature_2m_min" +
                  $"&forecast_days={days}" +
                  $"&timezone=auto";

        return await _httpClient.GetFromJsonAsync<ForecastResponse>(url);
    }
}
