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
        // public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, IMapper mapper, DataContext context)
        {
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request){
            var user = new User();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            // user.isAdmin = true;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request){
            var user = _context.Users.Single(u => u.Username == request.Username);

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
                new Claim(ClaimTypes.Name, user.Username),
                // new Claim(ClaimTypes.Role, "Admin"),
            };
            if(user.isAdmin){
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

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