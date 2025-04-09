using API2.Models;

namespace API2.Data
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly List<WeatherData> _weatherDataList = new List<WeatherData>();
        public void AddWeatherData(WeatherData weather)
        {
            _weatherDataList.Add(weather);
        }

        public List<WeatherData> GetAllWeatherData()
        {
            return _weatherDataList;
        }

        public List<WeatherData> GetLastNWeatherData(int n)
        {
            return _weatherDataList.Skip(_weatherDataList.Count - n < 0 ? 0 : _weatherDataList.Count - n).ToList();
        }
    }
}
