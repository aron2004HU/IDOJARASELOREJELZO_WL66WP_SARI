using api.Data;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherApiController : ControllerBase
    {
        private readonly WeatherForecastService _weatherService;

        public WeatherApiController(WeatherForecastService weatherService)
        {
            _weatherService = weatherService;
        }

        // POST /WeatherApi/forecast
        [HttpPost("forecast")]
        public IActionResult GetForecast([FromBody] List<WeatherData> lastThreeDays)
        {
            if (lastThreeDays == null || lastThreeDays.Count != 3)
                return BadRequest("Három nap adatai szükségesek.");

            ForecastResult forecast = _weatherService.CalculateForecast(lastThreeDays);
            return Ok(forecast);
        }
    }
}
