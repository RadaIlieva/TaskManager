using TaskManagerData.Entities;
using AutenticationAutorizationAPI.Models;

namespace AutenticationAutorizationAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Employee> Register(RegisterUserDto request);
        Task<string> Login(LoginUserDto request);
    }
}
