using Microsoft.AspNetCore.Mvc;
using TaskManagerProject.DTOs;
using TaskManagerProject.Services.Interfaces;
using TaskManagerProject.Models;

namespace TaskManagerProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDto model)
        {
            if (ModelState.IsValid)
            {
                var token = await accountService.LoginAsync(model);

                if (token != null)
                {
                    // Save token to session or authentication cookie here
                    return RedirectToAction("Index", "Home"); // Redirect to some page after login
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDto model)
        {
            if (ModelState.IsValid)
            {
                var success = await accountService.RegisterAsync(model);

                if (success)
                {
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(string.Empty, "Registration failed.");
            }

            return View(model);
        }
    }
}
