namespace WeatherProxy.Models.Dtos;

public class HourPointDto
{
    public string Time { get; set; }
    public double Temperature { get; set; }
    public double Precipitation { get; set; }
    public double Wind { get; set; }
}
