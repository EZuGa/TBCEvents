using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace C_.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public AuthService(IConfiguration configuration, IMapper mapper, DataContext context)
        {
            _configuration = configuration;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<string>> Login(UserDto request)
        {
            var serviceResponse = new ServiceResponse<string>();
            var user = await _context.Users.SingleAsync(u => u.Username == request.Username);

            if(user.Username != request.Username){
                serviceResponse.Message = "User Not Found!";
                serviceResponse.Success = false;
                return serviceResponse;
            }

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)){
                serviceResponse.Message = "Password incorrect!";
                serviceResponse.Success = false;
                return serviceResponse; 
            }

            string token = CreateToken(user);
            serviceResponse.Data = token;
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> Register(UserDto request)
        {
            var serviceResponse = new ServiceResponse<string>();
            var user = new User();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            // user.isAdmin = true;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            serviceResponse.Data = "Regietered!";
            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> DeleteUser(int id)
        {
            var serviceResponse = new ServiceResponse<string>();
            // var tbcEvent = events.First(c => c.Id == id);
            var userToDelete = await _context.Users.FindAsync(id);


            _context.Users.Remove(userToDelete);

            await _context.SaveChangesAsync();

            serviceResponse.Data = "REMOVED!";

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<User>>> SeeAll()
        {
            var users = await _context.Users.ToListAsync();

            var serviceResponse = new ServiceResponse<List<User>>
            {
                Data = users
            };

            return serviceResponse;
        }

        public Task<ServiceResponse<string>> UpdateUser()
        {
            throw new NotImplementedException();
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