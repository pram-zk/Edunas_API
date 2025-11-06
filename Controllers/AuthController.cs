using API_Edunas.Data;
using API_Edunas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Edunas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            this._context = context;
            this.configuration = configuration;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] Users user)
        {
            if (_context.Users.Any(f => f.Username == user.Email))
                return BadRequest(new { message = "Username already exist" });

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new {user = user, message = "User registered succesfully"});
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            var user = _context.Users.FirstOrDefault(f => f.Email == login.Email && f.Password == login.Password);
            if (user == null)
                return BadRequest(new { message = "Invalid Credentials" });

            var token = GenerateJwtToken(user);
            return Ok(new { success = true, data = user, message = "Login Succesfully", token = token });
        }

        private string GenerateJwtToken(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("role", user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
