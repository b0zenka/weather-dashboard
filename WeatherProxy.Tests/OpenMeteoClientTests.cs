using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using WeatherProxy.Models.OpenMeteoResponses;
using WeatherProxy.Services;

namespace WeatherProxy.Tests;
public class OpenMeteoClientTests
{
    private HttpClient GetMockHttpClient(HttpResponseMessage responseMessage)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        return new HttpClient(handlerMock.Object);
    }

    [Fact]
    public async Task GeocodeAsync_ReturnsCoordinates_WhenCityFound()
    {
        // Arrange
        var mockResponse = new GeocodingResponse
        {
            results = new List<GeocodingResult>
            {
                new GeocodingResult
                {
                    latitude = 10.0,
                    longitude = 20.0,
                    name = "TestCity",
                    country = "TestCountry"
                }
            }
        };

        var httpClient = GetMockHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(mockResponse)
        });

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(f => f.CreateClient("openmeteo")).Returns(httpClient);

        var client = new OpenMeteoClient(httpClientFactoryMock.Object);

        // Act
        var (lat, lon) = await client.GeocodeAsync("TestCity");

        // Assert
        Assert.Equal(10.0, lat);
        Assert.Equal(20.0, lon);
    }

    [Fact]
    public async Task GeocodeAsync_ThrowsKeyNotFoundException_WhenCityNotFound()
    {
        // Arrange
        var mockResponse = new GeocodingResponse
        {
            results = null
        };

        var httpClient = GetMockHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(mockResponse)
        });

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(f => f.CreateClient("openmeteo")).Returns(httpClient);

        var client = new OpenMeteoClient(httpClientFactoryMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => client.GeocodeAsync("UnknownCity"));
    }

    [Fact]
    public async Task GetForecastAsync_ReturnsForecastResponse_WithExpectedData()
    {
        // Arrange
        var mockForecast = new ForecastResponse
        {
            timezone = "Europe/Warsaw",
            daily = new Daily
            {
                time = new List<string> { "2025-09-04" },
                temperature_2m_max = new List<double> { 25.5 },
                temperature_2m_min = new List<double> { 15.2 }
            },
            hourly = new Hourly
            {
                time = new List<string> { "2025-09-04T12:00" },
                temperature_2m = new List<double> { 22.3 },
                precipitation = new List<double> { 0.0 },
                wind_speed_10m = new List<double> { 5.4 }
            }
        };

        var httpClient = GetMockHttpClient(new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(mockForecast)
        });

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(f => f.CreateClient("openmeteo")).Returns(httpClient);

        var client = new OpenMeteoClient(httpClientFactoryMock.Object);

        // Act
        var result = await client.GetForecastAsync(10.0, 20.0, 1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Europe/Warsaw", result!.timezone);

        Assert.Single(result.daily.time);
        Assert.Equal(25.5, result.daily.temperature_2m_max[0]);
        Assert.Equal(15.2, result.daily.temperature_2m_min[0]);

        Assert.Single(result.hourly.time);
        Assert.Equal(22.3, result.hourly.temperature_2m[0]);
        Assert.Equal(0.0, result.hourly.precipitation[0]);
        Assert.Equal(5.4, result.hourly.wind_speed_10m[0]);
    }
}
