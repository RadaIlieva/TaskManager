using AutenticationAutorizationAPI.Models;
using AutenticationAutorizationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutenticationAutorizationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AccountController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto request)
        {
            try
            {
                var employee = await authenticationService.Register(request);
                return Ok(new { Username = employee.FirstName + " " + employee.LastName, UniqueCode = employee.UniqueCode });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto request)
        {
            try
            {
                var token = await authenticationService.Login(request);

                if (token == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                return Ok(new { Token = token });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
