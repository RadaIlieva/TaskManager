using TaskManagerData.Entities;
using AutenticationAutorizationAPI.Models;

namespace AutenticationAutorizationAPI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterAsync(RegisterUserDto request);
        Task<string> Login(LoginUserDto request);
    }
}
