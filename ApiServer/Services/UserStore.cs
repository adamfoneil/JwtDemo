using WeatherService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SharedServices
{
    public class UserStore
    {
        private readonly string _jwtSecret;

        public UserStore(IOptions<UserStoreOptions> options)
        {
            _jwtSecret = options.Value.JwtSecret;
        }

        public string Login(string userName, string password)
        {
            if (IsValidUser(userName, password))
            {
                return GetTokenInner(userName);
            }

            throw new Exception("Invalid account");
        }

        /// <summary>
        /// with ChatGPT help
        /// </summary>
        private string GetTokenInner(string userName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(GetSecurityKey(_jwtSecret), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static SymmetricSecurityKey GetSecurityKey(string secret) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        private bool IsValidUser(string userName, string password) =>
            userName.Equals("herbie") && password.Equals("hancock");               
    }
}