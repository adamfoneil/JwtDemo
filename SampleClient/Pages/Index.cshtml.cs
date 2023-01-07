using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;
using SampleClient.Interfaces;
using System.Text;
using WeatherService;

namespace SampleClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWeatherClient _api;        

        private const string SessionToken = "token";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;            

            _api = RestService.For<IWeatherClient>("https://localhost:7113/", new RefitSettings()
            {
                AuthorizationHeaderValueGetter = async () =>
                {
                    if (HttpContext.Session.TryGetValue(SessionToken, out byte[]? val))
                    {
                        var tokenStr = Encoding.UTF8.GetString(val);
                        return await Task.FromResult(tokenStr) ?? string.Empty;
                    }

                    return string.Empty;
                }
            });
        }

        [BindProperty]
        public string UserName { get; set; } = default!;

        [BindProperty]
        public string Password { get; set; } = default!;

        public IEnumerable<WeatherForecast> Forecast { get; set; } = Enumerable.Empty<WeatherForecast>();

        public async Task<RedirectResult> OnPostLoginAsync()
        {
            try
            {
                var result = await _api.LoginAsync(UserName, Password);
                HttpContext.Session.SetString(SessionToken, result);                
            }
            catch (Exception exc)
            {
                TempData["error"] = exc.Message;                
            }

            return Redirect("/");
        }

        public async Task OnGetAsync()
        {
            try
            {
                if (HttpContext.Session.TryGetValue(SessionToken, out byte[]? val))
                {
                    Forecast = await _api.GetForecastAsync();
                }                
            }
            catch (Exception exc)
            {
                TempData["error"] = exc.Message;
            }
        }
    }
}