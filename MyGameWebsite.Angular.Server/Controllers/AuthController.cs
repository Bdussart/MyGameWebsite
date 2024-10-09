using AngularApp1.Server.DTO;
using AngularApp1.Server.MockData;
using AngularApp1.Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AngularApp1.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTO request)
        {
            if (ModelState.IsValid)
            {
                var user = UserStore.Users.FirstOrDefault(u => u.Name == request.Name&& u.Password == request.Password);
                if (user == null)
                {
                    return Unauthorized("Invalid user credentials.");
                }
                var token = IssueToken(user);
                return Ok(new { Token = token });
            }
            return BadRequest("Invalid Request Body.");
        }

        private string IssueToken(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>
            {
                new Claim("user_id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };
            user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
            // Creates a new JWT token with specified parameters including issuer, audience, claims, expiration time, and signing credentials.
            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(1), // Token expiration set to 1 hour from the current time.
                signingCredentials: credentials);
            // Serializes the JWT token to a string and returns it.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
