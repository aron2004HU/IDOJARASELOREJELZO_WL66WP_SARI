using API2.Data;
using API2.Models;
using Microsoft.AspNetCore.Mvc;

namespace API2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        IWeatherRepository repo;

        public WeatherForecastController(IWeatherRepository repo)
        {
            this.repo = repo;
        }

        private static Random _random = new Random();

        [HttpPost("forecast")]
        public IActionResult Forecast([FromBody] List<WeatherData> pastWeather)
        {
            if (pastWeather == null || pastWeather.Count == 0)
            {
                return BadRequest("Nincsenek időjárási adatok.");
            }

            WeatherType forecastType;

            if (pastWeather.Count >= 3)
            {
                var lastThree = pastWeather.Skip(pastWeather.Count - 3)
                                           .Select(w => w.Type)
                                           .ToList();
                bool identical = lastThree.Distinct().Count() == 1;

                if (identical)
                {
                    double rnd = _random.NextDouble();
                    if (rnd < 0.70)
                    {
                        forecastType = lastThree[0];
                    }
                    else if (rnd < 0.70 + 0.20)
                    {
                        forecastType = ImproveWeather(lastThree[0]);
                    }
                    else
                    {
                        forecastType = WorsenWeather(lastThree[0]);
                    }
                }
                else
                {
                    WeatherType[] possible = new WeatherType[] { WeatherType.Napos, WeatherType.Borult, WeatherType.Esos, WeatherType.Havas };
                    forecastType = possible[_random.Next(possible.Length)];
                }
            }
            else
            {
                WeatherType[] possible = new WeatherType[] { WeatherType.Napos, WeatherType.Borult, WeatherType.Esos, WeatherType.Havas };
                forecastType = possible[_random.Next(possible.Length)];
            }

            double temperature = 0;
            int windSpeed = 0;
            switch (forecastType)
            {
                case WeatherType.Napos:
                    temperature = _random.Next(20, 36);
                    windSpeed = _random.Next(0, 21);
                    break;
                case WeatherType.Borult:
                    temperature = _random.Next(15, 26);
                    windSpeed = _random.Next(0, 16);
                    break;
                case WeatherType.Esos:
                    temperature = _random.Next(10, 21);
                    windSpeed = _random.Next(10, 31);
                    break;
                case WeatherType.Havas:
                    temperature = _random.Next(-5, 6);
                    windSpeed = _random.Next(5, 26);
                    break;
            }

            WeatherData result = new WeatherData
            {
                Temperature = temperature,
                Type = forecastType,
                WindSpeed = windSpeed
            };

            this.repo.AddWeatherData(result);

            return Ok(result);
        }
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

        [HttpGet]
        public IEnumerable<WeatherData> GetWeather()
        {
            return this.repo.GetAllWeatherData();
            
        }

        //[HttpGet("{n}")]
        //public List<WeatherData>? GetLatestWeather(int n)
        //{
        //    return this.repo.GetLastNWeatherData(n);
        //}

    }
}
