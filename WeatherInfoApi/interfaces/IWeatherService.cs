using WeatherInfoApi.Models;

namespace WeatherInfoApi.interfaces
{
    public interface IWeatherService
    {
        Task<CityWeatherDto?> GetCityWeatherAsync(string cityName);

    }
}
