using System.Net;
using System.Text;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Dictionary<string, string> _responses;

    public MockHttpMessageHandler()
    {
        _responses = new Dictionary<string, string>
        {
            {
                "https://api.openweathermap.org/data/2.5/weather?q=Yazd&appid=fake-key&units=metric",
                @"{
                    ""name"": ""Yazd"",
                    ""main"": { ""temp"": 35.06, ""humidity"": 8 },
                    ""wind"": { ""speed"": 2.06 },
                    ""coord"": { ""lat"": 31.8972, ""lon"": 54.3675 }
                }"
            },
            {
                "https://api.openweathermap.org/data/2.5/air_pollution?lat=31.8972&lon=54.3675&appid=fake-key",
                @"{
                    ""list"": [{
                        ""main"": { ""aqi"": 3 },
                        ""components"": {
                            ""co"": 106.8, ""no"": 0.07, ""no2"": 0.35,
                            ""o3"": 127.74, ""so2"": 0.73,
                            ""pm2_5"": 11.02, ""pm10"": 27.49, ""nh3"": 1.01
                        }
                    }]
                }"
            }
        };
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_responses.TryGetValue(request.RequestUri.ToString(), out var content))
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });
        }

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("Not found")
        });
    }
}
