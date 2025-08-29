namespace WeatherProxy.Models.OpenMeteoResponses;

public class Daily
{
    public List<string> time { get; set; } = new();
    public List<double> temperature_2m_max { get; set; } = new();
    public List<double> temperature_2m_min { get; set; } = new();
}
