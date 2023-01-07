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
        private readonly TokenGenerator _tokenGenerator;

        public Account(TokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            try
            {
                var token = _tokenGenerator.GetToken(userName, password);
                return Ok(token);
            }
            catch (Exception exc)
            {
                return BadRequest(exc.ToString());
            }
        }
    }
}
