using WeatherProxy.Models.Dtos;

namespace WeatherProxy.Services
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeatherAsync(string city, int days);
    }
}