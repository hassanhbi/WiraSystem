using System.Net;
using System.Net.Http;
using System.Text.Json;
using WeatherInfoApi.interfaces;
using WeatherInfoApi.Models;


namespace WeatherInfoApi.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiKey = config["OpenWeather:ApiKey"];

        }
        public async Task<CityWeatherDto> GetCityWeatherAsync(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_apiKey}&units=metric";

            var result = await _httpClient.GetAsync(url);

            if (!result.IsSuccessStatusCode)
            {
                switch (result.StatusCode)
                {
                    case (HttpStatusCode.NotFound):
                        throw new Exception("شهر مورد نظر یافت نشد");

                    case (HttpStatusCode.Unauthorized):
                        throw new Exception("مدت اعتبار شما رو به اتمام و یا احراز هویت انجام نشد");

                    case (HttpStatusCode.BadRequest):
                        throw new Exception("درخواست نادرست به سرور ارسال شده است");

                    case (HttpStatusCode.InternalServerError):
                        throw new Exception("خطای سرور .لطفا بعدا تلاش کنید");

                    default:
                        throw new Exception($":{result.StatusCode}خطای ناشناخته");



                }
            }

            var response = await _httpClient.GetFromJsonAsync<JsonElement>(url);









            var main = response.GetProperty("main");
            var wind = response.GetProperty("wind");
            var coord = response.GetProperty("coord");

            var dto = new CityWeatherDto
            {
                CityName = response.GetProperty("name").GetString(),
                TemperatureCelsius = main.GetProperty("temp").GetDouble(),
                Humidity = main.GetProperty("humidity").GetInt32(),
                WindSpeed = wind.GetProperty("speed").GetDouble(),
                Latitude = coord.GetProperty("lat").GetDouble(),
                Longitude = coord.GetProperty("lon").GetDouble(),
                Pollutants = new Dictionary<string, double>()
            };

            try
            {
                var lat = dto.Latitude;
                var lon = dto.Longitude;

                var pollutionUrl = $"https://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={_apiKey}";
                var pollutionResponse = await _httpClient.GetFromJsonAsync<JsonElement>(pollutionUrl);

                dto.AQI = pollutionResponse.GetProperty("list")[0].GetProperty("main").GetProperty("aqi").GetInt32();

                var components = pollutionResponse.GetProperty("list")[0].GetProperty("components");
                dto.Pollutants = components.EnumerateObject()
                    .ToDictionary(p => p.Name.ToUpper(), p => p.Value.GetDouble());
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Warning: Could not retrieve pollution data: {ex.Message}");
            }

            return dto;
        }




    }


}

