using api.Models;

namespace api.Data
{
    public class WeatherForecastService
    {
        private static readonly Random rnd = new Random();

        public ForecastResult CalculateForecast(List<WeatherData> lastThreeDays)
        {
            if (lastThreeDays == null || lastThreeDays.Count != 3)
                throw new ArgumentException("Három nap adatai szükségesek.");

            // Számoljuk ki az átlagos hőmérsékletet és szélsebességet.
            float avgTemp = lastThreeDays.Average(d => d.Temperature);
            float avgWind = lastThreeDays.Average(d => d.WindSpeed);

            WeatherType forecastType;

            // Ha az előző három nap azonos időjárást mutat, alkalmazzuk a valószínűségi szabályokat.
            if (lastThreeDays.All(d => d.Type == lastThreeDays[0].Type))
            {
                int prob = rnd.Next(1, 101);
                if (prob <= 70)
                    forecastType = lastThreeDays[0].Type;  // 70% esély: marad azonos
                else if (prob <= 90)
                    forecastType = ImproveWeather(lastThreeDays[0].Type); // 20% esély: javulás
                else
                    forecastType = DeteriorateWeather(lastThreeDays[0].Type); // 10% esély: romlás
            }
            else
            {
                // Változatos adatok esetén véletlenszerűen választunk egy időjárási típust.
                Array types = Enum.GetValues(typeof(WeatherType));
                forecastType = (WeatherType)types.GetValue(rnd.Next(types.Length));
            }

            return new ForecastResult
            {
                Temperature = avgTemp,
                WindSpeed = avgWind,
                Type = forecastType
            };
        }

        // Segédfüggvény a "javulás" logikájához.
        private WeatherType ImproveWeather(WeatherType current)
        {
            return current switch
            {
                WeatherType.Esos => WeatherType.Borult,
                WeatherType.Borult => WeatherType.Napos,
                WeatherType.Napos => WeatherType.Napos, // már a legjobb állapot
                _ => current
            };
        }

        // Segédfüggvény a "romlás" logikájához.
        private WeatherType DeteriorateWeather(WeatherType current)
        {
            return current switch
            {
                WeatherType.Napos => WeatherType.Borult,
                WeatherType.Borult => WeatherType.Esos,
                WeatherType.Esos => WeatherType.Esos, // nem romlik tovább
                _ => current
            };
        }
    }
}
