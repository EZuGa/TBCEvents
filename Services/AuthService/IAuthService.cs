using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace C_.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(UserDto request);
        Task<ServiceResponse<string>> Login(UserDto request);
        Task<ServiceResponse<string>> DeleteUser();
        Task<ServiceResponse<string>> UpdateUser();
        Task<ServiceResponse<List<User>>> SeeAll();
    }
}