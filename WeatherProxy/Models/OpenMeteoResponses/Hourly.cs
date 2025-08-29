namespace WeatherProxy.Models.OpenMeteoResponses;

public class Hourly
{
    public List<string> time { get; set; } = new();
    public List<double> temperature_2m { get; set; } = new();
    public List<double> precipitation { get; set; } = new();
    public List<double> wind_speed_10m { get; set; } = new();
}
