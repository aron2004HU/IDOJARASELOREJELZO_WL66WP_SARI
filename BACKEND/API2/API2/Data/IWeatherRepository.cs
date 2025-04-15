using API2.Models;

namespace API2.Data
{
    public interface IWeatherRepository
    {
        void AddWeatherData(WeatherData weather);
        List<WeatherData> GetAllWeatherData();
        //List<WeatherData> GetLastNWeatherData(int n);
    }
}
