using C_.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace C_.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<string>>> Register(UserDto request){
            return Ok(await _authService.Register(request));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserDto request){
            return Ok(await _authService.Login(request));
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<ServiceResponse<string>>> Delete(int id){
            return Ok(await _authService.DeleteUser(id));
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetAll(){
            return Ok(await _authService.SeeAll());
        }
        
    }
}