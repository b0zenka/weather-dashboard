namespace WeatherProxy.Models.Dtos;

public class DailyPointDto
{
    public string Date { get; set; } = string.Empty;
    public double TemperatureMin { get; set; }
    public double TemperatureMax { get; set; }
}
