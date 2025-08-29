namespace WeatherProxy.Models.Dtos;

public class WeatherDto
{
    public string City { get; set; } = string.Empty;
    public CoordinatesDto Coordinates { get; set; } = new();
    public string Timezone {  get; set; } = string.Empty;
    public List<HourPointDto> Hourly { get; set; } = new();
    public List<DailyPointDto> Daily { get; set; } = new();
}