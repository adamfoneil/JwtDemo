using WeatherService;
using Refit;

namespace SampleClient.Interfaces
{
    [Headers("Authorization: Bearer")]
    internal interface IWeatherClient
    {
        [Post("/Account")]
        public Task<string> LoginAsync([Query]string userName, [Query]string password);
        
        [Get("/WeatherForecast")]
        public Task<IEnumerable<WeatherForecast>> GetForecastAsync();
    }
}
