namespace WeatherProxy.Models.OpenMeteoResponses;

public class GeocodingResult
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string name { get; set; } = string.Empty;
    public string country { get; set; } = string.Empty;
}
