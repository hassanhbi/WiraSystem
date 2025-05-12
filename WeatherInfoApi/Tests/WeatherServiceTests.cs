using WeatherInfoApi.Services;
using Xunit;

namespace WeatherInfoApi.Tests
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task GetCityWeatherAsync_Yazd_ReturnsCorrectData()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var httpClient = new HttpClient(handler);

            var config = new ConfigurationBuilder().AddInMemoryCollection(
                new Dictionary<string, string> { { "OpenWeather:ApiKey", "fake-key" } }
            ).Build();

            var service = new WeatherService(httpClient, config);

            // Act
            var result = await service.GetCityWeatherAsync("Yazd");

            // Assert
            Assert.Equal("Yazd", result.CityName);
            Assert.Equal(35.06, result.TemperatureCelsius);
            Assert.Equal(8, result.Humidity);
            Assert.Equal(2.06, result.WindSpeed);
            Assert.Equal(3, result.AQI);
            Assert.True(result.Pollutants.ContainsKey("CO"));
            Assert.Equal(106.8, result.Pollutants["CO"]);
        }
    }

}
