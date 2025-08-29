namespace WeatherProxy.Models.OpenMeteoResponses;

public class ForecastResponse
{
    public string timezone { get; set; } = string.Empty;
    public Daily daily { get; set; } = new();
    public Hourly hourly { get; set; } = new();
}
