using WeatherProxy.Models.OpenMeteoResponses;

namespace WeatherProxy.Services
{
    public interface IOpenMeteoClient
    {
        Task<(double latitude, double longitude)> GeocodeAsync(string city);
        Task<ForecastResponse?> GetForecastAsync(double latitude, double longitude, int days);
    }
}