using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedServices;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class Account : Controller
    {
        private readonly UserStore _userStore;

        public Account(UserStore userStore)
        {
            _userStore = userStore;
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            try
            {
                var token = _userStore.Login(userName, password);
                return Ok(token);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.ToString());
            }
        }
    }
}
