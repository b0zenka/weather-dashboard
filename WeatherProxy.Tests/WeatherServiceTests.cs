using Microsoft.Extensions.Caching.Memory;
using Moq;
using WeatherProxy.Models.Dtos;
using WeatherProxy.Models.OpenMeteoResponses;
using WeatherProxy.Services;

namespace WeatherProxy.Tests;
public class WeatherServiceTests
{
    private readonly IMemoryCache _cache;

    public WeatherServiceTests()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    [Fact]
    public async Task GetWeatherAsync_ReturnsFromCache_WhenAvailable()
    {
        // Arrange
        var cachedWeather = new WeatherDto { City = "CachedCity", Timezone = "UTC" };
        _cache.Set("CachedCity:3", cachedWeather);

        var clientMock = new Mock<IOpenMeteoClient>();
        var service = new WeatherService(clientMock.Object, _cache);

        // Act
        var result = await service.GetWeatherAsync("CachedCity", 3);

        // Assert
        Assert.Equal("CachedCity", result.City);
        Assert.Equal("UTC", result.Timezone);
        clientMock.Verify(c => c.GeocodeAsync(It.IsAny<string>()), Times.Never);
        clientMock.Verify(c => c.GetForecastAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetWeatherAsync_FetchesAndCaches_WhenNotInCache()
    {
        // Arrange
        var clientMock = new Mock<IOpenMeteoClient>();
        clientMock.Setup(c => c.GeocodeAsync("Warsaw")).ReturnsAsync((52.23, 21.01));
        clientMock.Setup(c => c.GetForecastAsync(52.23, 21.01, 2)).ReturnsAsync(new ForecastResponse
        {
            timezone = "Europe/Warsaw",
            daily = new Daily
            {
                time = new List<string> { "2025-09-04" },
                temperature_2m_max = new List<double> { 25 },
                temperature_2m_min = new List<double> { 15 }
            },
            hourly = new Hourly
            {
                time = new List<string> { "2025-09-04T12:00" },
                temperature_2m = new List<double> { 20 },
                precipitation = new List<double> { 0 },
                wind_speed_10m = new List<double> { 5 }
            }
        });

        var service = new WeatherService(clientMock.Object, _cache);

        // Act
        var result = await service.GetWeatherAsync("Warsaw", 2);

        // Assert
        Assert.Equal("Warsaw", result.City);
        Assert.Equal("Europe/Warsaw", result.Timezone);
        Assert.Single(result.Daily);
        Assert.Single(result.Hourly);

        // Ensure it is cached
        Assert.True(_cache.TryGetValue("Warsaw:2", out WeatherDto cached));
        Assert.Equal(result, cached);

        // Verify external calls
        clientMock.Verify(c => c.GeocodeAsync("Warsaw"), Times.Once);
        clientMock.Verify(c => c.GetForecastAsync(52.23, 21.01, 2), Times.Once);
    }

    [Fact]
    public async Task GetWeatherAsync_ThrowsException_WhenForecastUnavailable()
    {
        // Arrange
        var clientMock = new Mock<IOpenMeteoClient>();
        clientMock.Setup(c => c.GeocodeAsync("Nowhere")).ReturnsAsync((0, 0));
        clientMock.Setup(c => c.GetForecastAsync(0, 0, 1)).ReturnsAsync((ForecastResponse?)null);

        var service = new WeatherService(clientMock.Object, _cache);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.GetWeatherAsync("Nowhere", 1));

        // Verify calls
        clientMock.Verify(c => c.GeocodeAsync("Nowhere"), Times.Once);
        clientMock.Verify(c => c.GetForecastAsync(0, 0, 1), Times.Once);
    }

    [Fact]
    public async Task GetWeatherAsync_SecondCall_UsesCache()
    {
        // Arrange
        var clientMock = new Mock<IOpenMeteoClient>();
        clientMock.Setup(c => c.GeocodeAsync("Berlin")).ReturnsAsync((52.52, 13.41));
        clientMock.Setup(c => c.GetForecastAsync(52.52, 13.41, 1)).ReturnsAsync(new ForecastResponse
        {
            timezone = "Europe/Berlin",
            daily = new Daily
            {
                time = new List<string> { "2025-09-04" },
                temperature_2m_max = new List<double> { 23 },
                temperature_2m_min = new List<double> { 14 }
            },
            hourly = new Hourly
            {
                time = new List<string> { "2025-09-04T15:00" },
                temperature_2m = new List<double> { 19 },
                precipitation = new List<double> { 0.1 },
                wind_speed_10m = new List<double> { 4 }
            }
        });

        var service = new WeatherService(clientMock.Object, _cache);

        // Act: first call (should hit the client)
        var firstResult = await service.GetWeatherAsync("Berlin", 1);

        // Act: second call (should use cache)
        var secondResult = await service.GetWeatherAsync("Berlin", 1);

        // Assert
        Assert.Equal(firstResult.City, secondResult.City);
        Assert.Equal(firstResult.Timezone, secondResult.Timezone);

        // Verify client was only called once
        clientMock.Verify(c => c.GeocodeAsync("Berlin"), Times.Once);
        clientMock.Verify(c => c.GetForecastAsync(52.52, 13.41, 1), Times.Once);
    }
}
