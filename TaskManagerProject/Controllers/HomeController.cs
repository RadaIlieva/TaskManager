using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Profile action to show user details
        public IActionResult Profile()
        {
            // Add logic to retrieve and display user profile information
            return View();
        }

        // New Project action
        public IActionResult NewProject()
        {
            // Add logic to create a new project
            return View();
        }
    }
}
