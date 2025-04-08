namespace api.Models
{
    public class ForecastResult
    {
        public float Temperature { get; set; }
        public WeatherType Type { get; set; }
        public float WindSpeed { get; set; }
    }
}
