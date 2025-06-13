using DataLayer.Entities.Users;
using DataLayer.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChessServer.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController()
        {
        }

        [NonAction]
        public string CreateToken(User user, IConfiguration configuration)
        {
            // create claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // fetch secret from configuration
            var secret = configuration.GetSection("Auth:Secret").Value;

            // create a security key using the secret
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var handler = new JwtSecurityTokenHandler();
            string tokenString = handler.WriteToken(token);
            return tokenString;
        }

        [NonAction]
        public JwtSecurityToken decodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = handler.ReadJwtToken(token);

            return jwtToken;
        }
    }
}
