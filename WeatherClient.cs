using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace HelloDotnet5
{
    public class WeatherClient
    {
       private readonly HttpClient _httpClient;
       public readonly ServiceSettings _settings;

        public WeatherClient(HttpClient httpClient, IOptions<ServiceSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<Forecast> GetCurrentWeatherAsync(string city)
        {
            var forecast = await _httpClient.GetFromJsonAsync<Forecast>($"https://{_settings.OpenWeatherHost}/data/2.5/weather?q={city}&appid={_settings.ApiKey}");
            return forecast;
        }
        
        // Record Types --> for avoing creating types/classes
        public record Weather(string description);
        public record Main(decimal temp);
        public record Forecast(Weather[] weather, Main main, long dt);

    }
}