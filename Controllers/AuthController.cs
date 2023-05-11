using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using C_.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace C_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request){
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request){
            if(user.Username != request.Username){
                return BadRequest("User Not Found!");
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)){
                return BadRequest("User Not Found! (Password)");
            }

            string token = CreateToken(user);
            
            return Ok(token);
        }
        
        private string CreateToken(User user){
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}