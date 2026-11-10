using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Top5.Models;

namespace Top5.Controllers.API
{
    [ApiController]
    [Route("api/auth")]
    public class AuthApiController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _users;
        private readonly IConfiguration _config;

        public AuthApiController(UserManager<IdentityUser> user, IConfiguration config)
        {
            _users = user;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginInput input)
        {
            IdentityUser? user = await _users.FindByEmailAsync(input.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Wrong email or password" });
            }
            bool password_ok = await _users.CheckPasswordAsync(user, input.Password);
            if (!password_ok)
            {
                return Unauthorized(new { message = "Wrong email or password" });
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("sub", user.Id));
            claims.Add(new Claim("email", input.Email));
            IList<string> roles = await _users.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            string key_text = _config["Jwt:Key"]!;
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key_text));
            SigningCredentials signing = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.UtcNow.AddMinutes(60);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: signing);

            string token_text = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = token_text, expires = expires });
        }
    } 
}



