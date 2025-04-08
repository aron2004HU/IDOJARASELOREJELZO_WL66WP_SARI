namespace api.Models
{
    public enum WeatherType
    {
        Napos,
        Borult,
        Esos
    }
    public class WeatherData
    {
        public float Temperature { get; set; }
        public WeatherType Type { get; set; }
        public float WindSpeed { get; set; }
    }
}
