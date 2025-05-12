using Microsoft.AspNetCore.Mvc;
using WeatherInfoApi.interfaces;

namespace WeatherInfoApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        [HttpGet("{city}")]
        public async Task<IActionResult> GetWeatherConditions(string city)
        {
         var result= await _weatherService.GetCityWeatherAsync(city);
            if (result == null)
            {
                return NotFound("City not found");
            }
            return Ok(result);
        }
    }
}
