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
        private readonly UserStore _tokenGenerator;

        public Account(UserStore tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            try
            {
                var token = _tokenGenerator.Login(userName, password);
                return Ok(token);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.ToString());
            }
        }
    }
}
