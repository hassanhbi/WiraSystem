namespace WeatherInfoApi.Models
{
    public class CityWeatherDto
    {
        public string CityName { get; set; }

        //درجه حرارت بر حسب سانتیگراد
        public double TemperatureCelsius { get; set; }

        //رطوبت
        public int Humidity { get; set; }

        //سرعت باد
        public double WindSpeed { get; set; }
        //شاخص کیفیت هوا
        public int AQI { get; set; }


        //آلاینده ها :"pm10": 12.34 مثال  

        public Dictionary<string, double> Pollutants { get; set; } = new();

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
