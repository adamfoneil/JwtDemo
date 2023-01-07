using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Refit;
using SampleClient.Interfaces;
using SampleClient.Models;
using System.Text;
using WeatherService;

namespace SampleClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWeatherClient _api;
        private readonly ServerOptions _options;

        private const string SessionToken = "token";

        public IndexModel(ILogger<IndexModel> logger, IOptions<ServerOptions> options)
        {
            _logger = logger;
            _options = options.Value;

            _api = RestService.For<IWeatherClient>(_options.BaseUrl, new RefitSettings()
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

        public string ApiUrl => _options.BaseUrl;

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