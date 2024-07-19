using TaskManagerProject.DTOs;
using System.Threading.Tasks;

namespace TaskManagerProject.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterUserDto registerUserDto);
        Task<string> LoginAsync(LoginUserDto loginUserDto);
    }
}
