using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherProxy.Controllers;
using WeatherProxy.Models.Dtos;
using WeatherProxy.Services;

namespace WeatherProxy.Tests;
public class WeatherControllerTests
{
    [Fact]
    public async Task GetWeather_ReturnsOk_WhenWeatherAvailable()
    {
        // Arrange
        var mockService = new Mock<IWeatherService>();
        mockService.Setup(s => s.GetWeatherAsync("Paris", 3))
            .ReturnsAsync(new WeatherDto
            {
                City = "Paris",
                Timezone = "Europe/Paris",
                Coordinates = new CoordinatesDto { Latitude = 48.85, Longitude = 2.35 }
            });

        var controller = new WeatherController(mockService.Object);

        // Act
        var result = await controller.GetWeather("Paris");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var dto = Assert.IsType<WeatherDto>(okResult.Value);
        Assert.Equal("Paris", dto.City);
        Assert.Equal("Europe/Paris", dto.Timezone);
    }

    [Fact]
    public async Task GetWeather_ReturnsNotFound_WhenCityNotFound()
    {
        // Arrange
        var mockService = new Mock<IWeatherService>();
        mockService.Setup(s => s.GetWeatherAsync("Atlantis", 3))
            .ThrowsAsync(new KeyNotFoundException());

        var controller = new WeatherController(mockService.Object);

        // Act
        var result = await controller.GetWeather("Atlantis");

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result);

        var value = notFound.Value!;
        var messageProp = value.GetType().GetProperty("message");
        Assert.NotNull(messageProp);

        var message = messageProp!.GetValue(value)?.ToString();
        Assert.Contains("Atlantis", message);
    }

    [Fact]
    public async Task GetWeather_ReturnsBadGateway_WhenServiceThrows()
    {
        // Arrange
        var mockService = new Mock<IWeatherService>();
        mockService.Setup(s => s.GetWeatherAsync("Berlin", 5))
            .ThrowsAsync(new Exception("Service unavailable"));

        var controller = new WeatherController(mockService.Object);

        // Act
        var result = await controller.GetWeather("Berlin", 5);

        // Assert
        var badGateway = Assert.IsType<ObjectResult>(result);
        Assert.Equal(502, badGateway.StatusCode);

        var value = badGateway.Value!;
        var messageProp = value.GetType().GetProperty("message");
        Assert.NotNull(messageProp);

        var message = messageProp!.GetValue(value)?.ToString();
        Assert.Equal("Service unavailable", message);
    }

}
