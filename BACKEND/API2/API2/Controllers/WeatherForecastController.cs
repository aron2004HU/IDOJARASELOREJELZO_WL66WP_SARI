using API2.Models;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static Random _random = new Random();

        private WeatherType ImproveWeather(WeatherType current)
        {
            switch (current)
            {
                case WeatherType.Esos:
                    return WeatherType.Borult;
                case WeatherType.Borult:
                    return WeatherType.Napos;
                case WeatherType.Napos:
                    return WeatherType.Napos;
                case WeatherType.Havas:
                    return WeatherType.Borult;
                default:
                    return current;
            }
        }

        private WeatherType WorsenWeather(WeatherType current)
        {
            switch (current)
            {
                case WeatherType.Napos:
                    return WeatherType.Borult;
                case WeatherType.Borult:
                    return WeatherType.Esos;
                case WeatherType.Esos:
                    return WeatherType.Esos;
                case WeatherType.Havas:
                    return WeatherType.Esos;
                default:
                    return current;
            }
        }
    }
}
